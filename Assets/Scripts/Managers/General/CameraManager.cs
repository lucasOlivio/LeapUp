using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that controls the camera's movement and position in the game world.
/// </summary>
public class CameraManager : MonoBehaviour
{
    const int INITIAL_Z = -10;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;

        // Subscribe to the events
        GameManager.GameStart += GameStart;
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = GameManager.Player.transform.position;
        if (playerPos.y < transform.position.y) return;
        transform.position = new Vector3(0, playerPos.y, INITIAL_Z);
    }

    void GameStart()
    {
        transform.position = initialPosition;
    }
}
