using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameUtils
{
    /// <summary>
    /// Change the size in the given axis for the gameobject
    /// </summary>
    /// <param name="gameObject">GameObject to be changed.</param>
    /// <param name="deltaChange">Delta to change the size.</param>
    /// <param name="axis">Axis to change the size.</param>
    public static void ChangeSize(GameObject gameObject, float deltaChange, int axis)
    {
        SpriteRenderer spriteRenderer;
        try
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }
        catch
        {
            Debug.Log($"No SpriteRendered for the {gameObject.name} object!");
            return;
        }
        Vector2 size = spriteRenderer.size;
        size[axis] *= deltaChange;
        spriteRenderer.size = size;

        // Modify the collider size
        BoxCollider2D collider;
        try
        {
            collider = gameObject.GetComponent<BoxCollider2D>();
        }
        catch
        {
            Debug.Log($"No BoxCollider2D for the {gameObject.name} object!");
            return;
        }
        Vector2 colliderSize = collider.size;
        colliderSize[axis] *= deltaChange;
        collider.size = colliderSize; // Set the new width of the collider
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

    /// <summary>
    /// Iterates over every child element in the order of their "axis" position.
    /// </summary>
    public static Transform[] GetOrderedChildren(Transform parentTransform)
    {
        int childCount = parentTransform.childCount;

        // Create an array to store child transforms
        Transform[] children = new Transform[childCount];

        // Populate the array with child transforms
        for (int i = 0; i < childCount; i++)
        {
            children[i] = parentTransform.GetChild(i);
        }

        // Sort the children array based on their axis position
        System.Array.Sort(children, CompareAxisPosition);

        return children;
    }

    /// <summary>
    /// Comparison function to sort transforms based on their axis position.
    /// </summary>
    /// <param name="a">The first transform to compare.</param>
    /// <param name="b">The second transform to compare.</param>
    /// <returns>
    /// -1 if <paramref name="a"/> has a smaller "axis" position than <paramref name="b"/>,
    /// 1 if <paramref name="a"/> has a larger "axis" position than <paramref name="b"/>,
    /// 0 if <paramref name="a"/> and <paramref name="b"/> have the same "axis" position.
    /// </returns>
    public static int CompareAxisPosition(Transform a, Transform b)
    {
        // Compare the axis positions of two transforms
        if (a.position.x < b.position.x)
            return -1;
        else if (a.position.x > b.position.x)
            return 1;
        else
            return 0;
    }

}
