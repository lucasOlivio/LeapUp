using UnityEngine;

/// <summary>
/// Controls the movement and physics of the player character.
/// </summary>
public class Player_basic_rigid_body : MonoBehaviour
{   
    // UI
    public int highScore = 0;

    // Movement
    public float moveSpeed = 25f;

    // Jump
    public float jumpForce = 45f;
    public float gravityScale = 15f;
    private bool isGrounded = true;

    // Components
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        Move();

        HighScore();
    }

    private void FixedUpdate()
    {
        GravityForce();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }

    /// <summary>
    /// Moves the player left or right based on input.
    /// </summary>
    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float moveDirection = horizontalInput * moveSpeed;

        rb.velocity = new Vector2(moveDirection, rb.velocity.y);
    }

    /// <summary>
    /// Applies a gravity force to the player.
    /// </summary>
    private void GravityForce()
    {
        rb.velocity += Physics2D.gravity * (gravityScale - 1) * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Makes the player jump.
    /// </summary>
    private void Jump()
    {
        isGrounded = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    /// <summary>
    /// Updates the player's high score.
    /// </summary>
    private void HighScore()
    {
        float playerY = transform.position.y;

        if (playerY > highScore)
        {
            highScore = (int)playerY;
        }
    }
}
