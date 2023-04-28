using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{
    public GameObject groundPrefab; // Ground object to manage
    public Sprite groundBlockSprite; // Sprite referencing the normal ground
    public Sprite deathBlockSprite; // Sprite for the ground where player dies if touches
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    [SerializeField] private float incrementAmount;   // The amount to increment the Y position per second
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component attached to the Ground GameObject
        spriteRenderer = groundPrefab.GetComponent<SpriteRenderer>();

        initialPosition = transform.position;

        // Subscribe to the events
        GameManager.PlayerStart += PlayerStart;
        GameManager.GameStart += GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        FollowXPlayer();
    }

    private void FixedUpdate()
    {
        if(GameManager.isPlayable()) // If not playable, don't allow to move
            IncreaseY();
    }

    void FollowXPlayer() {
        float playerX = GameManager.Player.transform.position.x;

        Vector3 pos = groundPrefab.transform.position;
        pos.x = playerX;
        groundPrefab.transform.position = pos;
    }

    void IncreaseY() {
        if(GameManager.state != GameManager.GameStates.Playing) return;

        GameUtils.ChangePosition(this.gameObject, (incrementAmount * Time.fixedDeltaTime), 1);
    }

    void PlayerStart() {
        // Change the sprite to the death block
        spriteRenderer.sprite = deathBlockSprite;
    }

    void GameStart() {
        transform.position = initialPosition;
        // Change the sprite to the ground block
        spriteRenderer.sprite = groundBlockSprite;
    }
}
