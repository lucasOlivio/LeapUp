using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A script that controls the camera's movement and position in the game world.
/// </summary>
public class CameraScript_basic : MonoBehaviour
{
    [SerializeField] private Transform _target; // The object that the camera will follow
    [SerializeField] private Vector3 _offset = new Vector3(0, 3, -10); // The offset between the camera and the target object
    [SerializeField] private float _smoothTime = 0f; // The time it takes for the camera to reach its target position
    [SerializeField] private float _xMargin = 0.1f; // The horizontal distance the target object can move before the camera begins to follow
    [SerializeField] private float _yMargin = 0.1f; // The vertical distance the target object can move before the camera begins to follow
    [SerializeField] private float _xSpeed = 20f; // The speed at which the camera follows the target object horizontally
    [SerializeField] private float _ySpeed = 25f; // The speed at which the camera follows the target object vertically
    [SerializeField] public float _minY = 2; // The minimum y-position the camera can have
    [SerializeField] private float _maxY = float.PositiveInfinity; // The maximum y-position the camera can have

    private Vector3 _velocity = Vector3.zero; // The camera's current velocity

    private void FixedUpdate()
    {
        if (_target == null)
        {
            return;
        }

        FollowHorizontal();
        FollowVertical();
        ClampPosition();

        // Set the camera position
        Vector3 targetPosition = _target.position + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _smoothTime);
    }

    /// <summary>
    /// Updates the camera's x-position to follow the target object horizontally.
    /// </summary>
    private void FollowHorizontal()
    {
        float targetX = transform.position.x;
        if (_target.position.x > transform.position.x + _xMargin)
        {
            targetX = Mathf.Lerp(transform.position.x, _target.position.x - _xMargin, _xSpeed * Time.fixedDeltaTime);
        }
        else if (_target.position.x < transform.position.x - _xMargin)
        {
            targetX = Mathf.Lerp(transform.position.x, _target.position.x + _xMargin, _xSpeed * Time.fixedDeltaTime);
        }
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// Updates the camera's y-position to follow the target object vertically.
    /// </summary>
    private void FollowVertical()
    {
        float targetY = transform.position.y;
        if (_target.position.y > transform.position.y + _yMargin)
        {
            targetY = Mathf.Lerp(transform.position.y, _target.position.y - _yMargin, _ySpeed * Time.fixedDeltaTime);
        }
        else if (_target.position.y < transform.position.y - _yMargin)
        {
            targetY = Mathf.Lerp(transform.position.y, _target.position.y + _yMargin, _ySpeed * Time.fixedDeltaTime);
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }

    /// <summary>
    /// Clamps the camera's y-position to stay within the bounds of the game world.
    /// </summary>
    private void ClampPosition()
    {
        float clampedY = Mathf.Clamp(transform.position.y, _minY + _offset.y, _maxY - _offset.y);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }
}
