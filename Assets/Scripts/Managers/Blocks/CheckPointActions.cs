using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CheckPointActions : MonoBehaviour
{
    [SerializeField] private float platformSpeed;
    [SerializeField] private int[] rangesMove;
    [SerializeField] private float blinkTime;
    [SerializeField] private bool isActive;

    private int[] directionMove;
    private Vector3 initialPositionReset;
    private Vector3 preActionPosition;

    private float elapsedTime;
    private Block[] childActions;

    private void Start()
    {
        EventManager.GameOver += GameOver;
        initialPositionReset = transform.position;
        preActionPosition = transform.position;
        directionMove = new int[2] { 1, 1 };
        elapsedTime = 0f;

        childActions = GetComponentsInChildren<Block>();

        // TODO: Way to make this dynamic accordingly to the spacing of the platforms.
        // Set initial visibility based on global position to alternate platforms blinking
        isActive = (transform.position.y % 8) == 0;
    }

    float GetElapsedTime()
    {
        return elapsedTime;
    }

    Vector3 GetPreActionPosition()
    {
        return preActionPosition;
    }

    int[] GetDirectionMove()
    {
        return directionMove;
    }

    bool GetIsActive()
    {
        return isActive;
    }

    void SetOpposedParams(string functionName, Vector3 newPosition, CheckPointActions opposedReference)
    {
        Vector3 opposedPosition = opposedReference.transform.position;
        Vector3 opposedInitialPosition = opposedReference.GetPreActionPosition();
        int[] opposedDirections = opposedReference.GetDirectionMove();

        if (functionName == "MovePlatformX")
        {
            float opposedX = opposedInitialPosition.x - opposedPosition.x;

            transform.position = new Vector3(
                transform.position.x + opposedX,
                transform.position.y,
                transform.position.z
            );

            directionMove[0] = -opposedDirections[0];
        }

        if (functionName == "MovePlatformY")
        {
            float opposedY = opposedInitialPosition.y - opposedPosition.y;

            transform.position = new Vector3(
                transform.position.x,
                transform.position.y + opposedY,
                transform.position.z
            );

            directionMove[1] = -opposedDirections[1];
        }

        if (functionName == "BlinkPlatform")
        {
            bool opposedIsActive = opposedReference.GetIsActive();
            float opposedElapsedTime = opposedReference.GetElapsedTime();

            isActive = !opposedIsActive;
            elapsedTime = opposedElapsedTime;
        }

    }

    /// <summary>
    /// Triggers the specified functions if the object's position on the given axis 
    /// is greater than or equal to the checkpoint value.
    /// </summary>
    /// <param name="axis">The index of the axis (x, y, or z) to check the object's position.</param>
    /// <param name="checkpoint">The checkpoint value that determines if the functions should be triggered.</param>
    /// <param name="functionsList">A list of function names to be executed using coroutines.</param>
    /// <param name="newPosition">New position for the object, to avoid displacements.</param>
    /// <param name="opposedReference">Last object to intercalate position and blinks from.</param>
    public void OnCheckPoint(int checkpoint, List<string> functionsList, Vector3 newPosition, CheckPointActions opposedReference)
    {
        ResetPlatform();
        SetNewPosition(newPosition);
        foreach (string functionName in functionsList)
        {
            SetOpposedParams(functionName, newPosition, opposedReference);
            StartCoroutine(functionName);
        }
    }

    /// <summary>
    /// Moves the platform along a specified axis relative to its parent's coordinate system, forwards and backwards.
    /// </summary>
    /// <param name="axis">The index of the axis to move the platform on.</param>
    private void MovePlatform(int axis)
    {
        Vector3 position = transform.position;
        float positionAxis = position[axis];
        float initialPosAxis = preActionPosition[axis];
        float rangeAxis = rangesMove[axis];
        int direction = directionMove[axis];

        positionAxis += direction * (platformSpeed * Time.fixedDeltaTime);
        if (positionAxis >= initialPosAxis + rangeAxis)
            direction = -1;
        else if (positionAxis <= initialPosAxis - rangeAxis)
            direction = 1;

        directionMove[axis] = direction;
        position[axis] = positionAxis;
        transform.position = position;
    }

    IEnumerator MovePlatformX()
    {
        while (true)
        {
            MovePlatform(0);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator MovePlatformY()
    {
        while (true)
        {
            MovePlatform(1);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator BlinkPlatform()
    {
        while (true)
        {
            elapsedTime += Time.fixedDeltaTime;

            if (elapsedTime >= blinkTime)
            {
                isActive = !isActive;
                elapsedTime = 0f;

                SetChildrenActive(isActive);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    void SetChildrenActive(bool isActive)
    {
        foreach (Block child in childActions)
        {
            child.SetActive(isActive);
        }
    }

    void ResetPlatform()
    {
        StopAllCoroutines();
        SetChildrenActive(true);
        elapsedTime = 0f;
    }

    void SetNewPosition(Vector3 newPos)
    {
        transform.position = newPos;
        preActionPosition = newPos;
    }

    void GameOver()
    {
        ResetPlatform();
        SetNewPosition(initialPositionReset);
    }
}
