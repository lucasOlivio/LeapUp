using UnityEngine;

/// <summary>
/// A script that controls the camera's movement and position in the game world.
/// </summary>
public class CameraManager : MonoBehaviour
{
    public float offsetY;
    const int INITIAL_Z = -10;
    private Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.position;

        // Subscribe to the events
        EventManager.GameStart += GameStart;
    }

    private void FixedUpdate()
    {
        Vector3 playerPos = GameManager.GetPlayerPosition();
        float cameraY = playerPos.y + offsetY;
        if (cameraY < transform.position.y) return;
        transform.position = new Vector3(0, cameraY, INITIAL_Z);
    }

    void GameStart()
    {
        transform.position = initialPosition;
    }
}
