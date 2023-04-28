using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the creation and movement of platforms vertically to follow the player Y.
/// </summary>
public class PlatformsManager : EndlessManager
{
    private void Awake()
    {
        axis = 1;
    }

    /// <summary>
    /// Instantiates a platformManager object at the specified position.
    /// </summary>
    /// <param name="posAxis">The y position of the platform.</param>
    /// <returns>The new platformManager object.</returns>
    protected override GameObject CreateBlock(float posAxis) {
        GameObject newObj = base.CreateBlock(posAxis);
        BlocksManager bm = newObj.GetComponent<BlocksManager>();

        bm.SetHeight(posAxis);
        bm.offset = (posAxis % 2) * (bm.spacing / 2);

        return newObj;
    }

    /// <summary>
    /// Move the platform up or down to the ends of the list depending on the floor direction to move.
    /// </summary>
    /// <param name="direction">The direction the floor should be moved.</param>
    protected override GameObject MoveBlock(int direction) {
        GameObject obj = base.MoveBlock(direction);
        BlocksManager bm = obj.GetComponent<BlocksManager>();

        float newY = obj.transform.position[axis];
        bm.SetHeight(newY);

        return obj;
    }
}
