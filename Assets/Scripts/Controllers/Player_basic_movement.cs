using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_basic : MonoBehaviour
{
    // UI
    public int score = 0;

    // Jump movement
    public float jumpForce = 10f; // The force with which the player jumps
    public int gravity = -50;
    public float maxHoldJumpTime = 0.4f;
    private float _jumpTimer = .0f;
    private Vector2 _velocity;
    private bool _isGrounded; // A flag that indicates whether the player is on the ground
    private bool _isHoldingJump; // A flag that indicates whether the player is holding the jump button
    private float _groundHeight = 1;

    // Walk movement
    public float moveSpeed = 5f; // The speed that the player can move horizontally
    private float _moveHorizontal; // Which direction player should go

    // Start is called before the first frame update
    void Start()
    {
        _isGrounded = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckJump();
    }

    private void FixedUpdate()
    {
        Jump();
        Walk();
        UpdateScore();
    }

    void Walk()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 pos = transform.position;

        // Move left or right
        pos.x += horizontalInput * moveSpeed * Time.fixedDeltaTime;

        transform.position = pos;
    }

    void CheckJump()
    {
        if (_isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _isGrounded = false;
                _isHoldingJump = true;
                _velocity.y = jumpForce;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _isHoldingJump = false;
        }
    }

    void Jump()
    {
        Vector2 pos = transform.position;

        if (_isGrounded)
            return;

        if (_isHoldingJump)
        {
            _jumpTimer += Time.fixedDeltaTime;
            if (_jumpTimer >= maxHoldJumpTime)
            {
                _isHoldingJump = false;
            }
        }

        pos.y += _velocity.y * Time.fixedDeltaTime;

        if (!_isHoldingJump)
            _velocity.y += gravity * Time.fixedDeltaTime;

        // Ground check
        if (pos.y <= _groundHeight)
        {
            pos.y = _groundHeight;
            _isGrounded = true;
            _jumpTimer = .0f;
        }

        transform.position = pos;
    }

    void UpdateScore()
    {
        float playerY = transform.position.y;

        if (playerY > score)
            score = (int)playerY;
    }
}
