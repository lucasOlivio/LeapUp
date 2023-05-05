using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private float blockSpeed = 1f;
    [SerializeField] private float rangeX = 2f;
    [SerializeField] private float rangeY = 1f;
    [SerializeField] private float blinkTime = 1f;

    private bool isMovingRight;
    private bool isMovingUp;
    private bool isBlockActive;
    private Vector3 initialPosition;
    private float elapsedTime;

    private void Start()
    {
        initialPosition = transform.position;
        isMovingRight = true;
        isBlockActive = true;
        elapsedTime = 0f;
    }

    private void FixedUpdate()
    {
        // MoveBlockX();
        // MoveBlockY();
        // BlinkBlock();
    }

    public void UpdateInitialPosition()
    {
        initialPosition = transform.position;
    }

    private void MoveBlockX()
    {
        Vector3 position = transform.position;
        int direction = -1;
        if (isMovingRight)
            direction = 1;

        position.x += direction * (blockSpeed * Time.fixedDeltaTime);
        if (position.x >= initialPosition.x + rangeX)
            isMovingRight = false;
        if (position.x <= initialPosition.x - rangeX)
            isMovingRight = true;

        transform.position = position;
    }

    private void MoveBlockY()
    {
        Vector3 position = transform.position;
        int direction = -1;
        if (isMovingUp)
            direction = 1;

        position.y += direction * (blockSpeed * Time.fixedDeltaTime);
        if (position.y >= initialPosition.y + rangeY)
            isMovingUp = false;
        if (position.y <= initialPosition.y - rangeY)
            isMovingUp = true;

        transform.position = position;
    }

    private void BlinkBlock()
    {
        if (!isBlockActive)
        {
            elapsedTime += Time.fixedDeltaTime;

            if (elapsedTime >= blinkTime)
            {
                isBlockActive = true;
                elapsedTime = 0f;
                gameObject.SetActive(true);
            }
        }
        else
        {
            elapsedTime += Time.fixedDeltaTime;

            if (elapsedTime >= blinkTime)
            {
                isBlockActive = false;
                elapsedTime = 0f;
                gameObject.SetActive(false);
            }
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
}
