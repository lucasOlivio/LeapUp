using UnityEngine;

/// <summary>
/// Controls the movement and physics of the player character.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // UI
    public int score = 0;

    // Movement
    public float moveSpeed;
    private float screenCenterX;

    // Jump
    public float startHeight;
    public float jumpForce;
    public float gravityScale;
    private bool _isGrounded;
    private Vector3 initialPosition;

    // Components
    private Rigidbody2D _rb;
    private FixedJoint2D fixedJoint;

    private void Start()
    {
        fixedJoint = GetComponent<FixedJoint2D>();

        _rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        score = 0;

        // Subscribe to the events
        EventManager.GameStart += GameStart;

        // save the horizontal center of the screen
        screenCenterX = Screen.width * 0.5f;
    }

    private void Update()
    {
        if (GameManager.isPlayable()) // If not playable, don't allow to move
            HandleInput();

        UpdateScore();
    }

    private float GetTouchSide()
    {
        // if there are any touches currently
        if (Input.touchCount == 0)
            return Input.GetAxisRaw("Horizontal");


        // get the first one
        Touch firstTouch = Input.GetTouch(0);

        // if it began this frame
        if (firstTouch.position.x > screenCenterX)
        {
            return 1;
        }
        else if (firstTouch.position.x < screenCenterX)
        {
            return -1;
        }

        return Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        ApplyGravity();

        if (transform.position.y > startHeight && GameManager.state == GameManager.GameStates.MainMenu)
        {
            EventManager.FirePlayerStartEvent();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateGroundedState(collision);

        if (collision.gameObject.CompareTag("Ground") && GameManager.state == GameManager.GameStates.Playing)
        {
            EventManager.FireGameOverEvent();
        }
    }

    /// <summary>
    /// Handles the player's input for movement and jumping.
    /// </summary>
    private void HandleInput()
    {
        Move();
        Jump();
    }

    /// <summary>
    /// Moves the player left or right based on input.
    /// </summary>
    private void Move()
    {
        float horizontalInput = GetTouchSide();
        float moveDirection = horizontalInput * moveSpeed;

        _rb.velocity = new Vector2(moveDirection, _rb.velocity.y);
    }

    /// <summary>
    /// Applies a gravity force to the player.
    /// </summary>
    private void ApplyGravity()
    {
        _rb.velocity += Physics2D.gravity * (gravityScale - 1) * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Makes the player jump if the jump button is pressed and the player is grounded.
    /// </summary>
    private void Jump()
    {
        if (_isGrounded)
        {
            _isGrounded = false;
            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }
    }

    /// <summary>
    /// Updates the player's grounded state based on the collision normal.
    /// </summary>
    private void UpdateGroundedState(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            _isGrounded = true;
        }
    }

    /// <summary>
    /// Updates the player's high score based on their current y position.
    /// </summary>
    private void UpdateScore()
    {
        float playerY = transform.position.y;

        if (playerY > score)
        {
            score = (int)playerY;
        }
    }

    void GameStart()
    {
        transform.position = initialPosition;
        score = 0;
        _isGrounded = false;
    }
}
