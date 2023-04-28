using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CustomExceptions;

public static class GameUtils
{
    /// <summary>
    /// Change the scale in the given axis for the gameobject
    /// </summary>
    /// <param name="gameObject">GameObject to be changed.</param>
    /// <param name="deltaChange">Delta to change the scale.</param>
    /// <param name="axis">Axis to change the scale.</param>
    public static void ChangeScale(GameObject gameObject, float deltaChange, int axis)
    {
        Vector3 scale = gameObject.transform.localScale;
        scale[axis] *= deltaChange;
        gameObject.transform.localScale = scale;
    }

    /// <summary>
    /// Change only the given axis for the gameobject
    /// </summary>
    /// <param name="gameObject">GameObject to be changed.</param>
    /// <param name="deltaChange">Delta to change from the position.</param>
    /// <param name="axis">Axis to change the position.</param>
    public static void ChangePosition(GameObject gameObject, float deltaChange, int axis)
    {
        Vector3 currentPosition = gameObject.transform.position;
        currentPosition[axis] += deltaChange;
        gameObject.transform.position = currentPosition;
    }

    /// <summary>
    /// Recursively searches for a nested GameObject by name within a parent GameObject.
    /// </summary>
    /// <param name="parentObject">The parent GameObject from which to start the search.</param>
    /// <param name="targetObjectName">The name of the GameObject to find.</param>
    /// <returns>The GameObject with the matching name, or null if not found.</returns>
    public static GameObject FindNestedGameObject(string parentObjectName, string targetObjectName)
    {
        GameObject parentObject = GameObject.Find(parentObjectName);

        // Get all child transforms of the parent GameObject
        Transform[] allChildTransforms = parentObject.GetComponentsInChildren<Transform>(true);

        // Iterate through each child transform
        foreach (Transform childTransform in allChildTransforms)
        {
            // Check if the name matches the targetObjectName
            if (childTransform.name == targetObjectName)
            {
                // Return the matching GameObject
                return childTransform.gameObject;
            }
        }

        // Target GameObject not found, throw an exception
        throw new GameObjectNotFoundException($"Could not find GameObject with name '{targetObjectName}'");
    }

}
