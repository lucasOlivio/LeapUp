using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] private float blockSpeed = 1f;
    [SerializeField] private int[] rangesMove;
    [SerializeField] private float blinkTime = 1f;

    private int[] directionMove;
    private bool isBlockActive;
    private Vector3 initialPosition;
    private float elapsedTime;

    private Collider2D objCollider;
    private SpriteRenderer sprite;

    private void Start()
    {
        initialPosition = transform.localPosition;
        directionMove = new int[2] { 1, 1 };
        isBlockActive = true;
        elapsedTime = 0f;
        objCollider = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        // MoveBlockX();
        // MoveBlockY();
        BlinkBlock();
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

    private void MoveBlockX()
    {
        MoveBlock(0);
    }

    private void MoveBlockY()
    {
        MoveBlock(1);
    }

    private void BlinkBlock()
    {
        elapsedTime += Time.fixedDeltaTime;

        if (elapsedTime >= blinkTime)
        {
            isBlockActive = !isBlockActive;
            elapsedTime = 0f;

            // Disable collisions
            objCollider.enabled = isBlockActive;

            // Hide the sprite
            sprite.enabled = isBlockActive;
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
