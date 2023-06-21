using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private float blockSpeed;
    [SerializeField] private int[] rangesMove;
    [SerializeField] private float blinkTime;
    [SerializeField] private bool isActive;
    private bool isActiveCopy;

    private int[] directionMove;
    private Vector3 initialPosition;

    private float elapsedTime;
    private Collider2D objCollider;
    private SpriteRenderer sprite;

    private void Start()
    {
        EventManager.GameOver += GameOver;
        initialPosition = transform.localPosition;
        directionMove = new int[2] { 1, 1 };
        elapsedTime = 0f;

        objCollider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

        // TODO: Way to make this dynamic accordingly to the spacing of the platforms.
        // Set initial visibility based on global position to alternate platforms blinking
        isActive = (transform.position.y % 8) == 0;
        isActiveCopy = isActive;
    }

    /// <summary>
    /// Moves the block along a specified axis relative to its parent's coordinate system, forwards and backwards.
    /// </summary>
    /// <param name="axis">The index of the axis to move the block on.</param>
    private void MoveBlock(int axis)
    {
        Vector3 position = transform.localPosition;
        float positionAxis = position[axis];
        float initialPosAxis = initialPosition[axis];
        float rangeAxis = rangesMove[axis];
        int direction = directionMove[axis];

        positionAxis += direction * (blockSpeed * Time.fixedDeltaTime);
        if (positionAxis >= initialPosAxis + rangeAxis)
            direction = -1;
        else if (positionAxis <= initialPosAxis - rangeAxis)
            direction = 1;

        directionMove[axis] = direction;
        position[axis] = positionAxis;
        transform.localPosition = position;
    }

    IEnumerator MoveBlockX()
    {
        while (true)
        {
            MoveBlock(0);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator MoveBlockY()
    {
        while (true)
        {
            MoveBlock(1);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator BlinkBlock()
    {
        while (true)
        {
            elapsedTime += Time.fixedDeltaTime;

            if (elapsedTime >= blinkTime)
            {
                isActive = !isActive;
                elapsedTime = 0f;

                // Disable collisions
                objCollider.enabled = isActive;

                // Hide the sprite
                sprite.enabled = isActive;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    void ResetBlock()
    {
        StopAllCoroutines();

        // Returns all initial states for the block
        isActive = isActiveCopy;
        objCollider.enabled = true;
        sprite.enabled = true;
        transform.localPosition = initialPosition;
    }

    public void OnCheckPointEvent(int axis, int currentCheckpoint, List<string> functionsList)
    {
        ResetBlock();
        foreach (string functionName in functionsList)
        {
            StartCoroutine(functionName);
        }
    }

    void GameOver()
    {
        ResetBlock();
    }
}
