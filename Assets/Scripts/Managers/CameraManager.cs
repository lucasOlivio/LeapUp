using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that controls the camera's movement and position in the game world.
/// </summary>
public class CameraScript : MonoBehaviour
{
    const int INITIAL_Z = -10;

    private void FixedUpdate()
    {
        Vector3 playerPos = GameManager.Player.transform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, INITIAL_Z);
    }
}
