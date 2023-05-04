using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the creation and movement of platforms vertically to follow the player Y.
/// </summary>
public class YManager : EndlessManager
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
    protected override GameObject AfterCreateBlock(GameObject newObj)
    {
        XManager bm = newObj.GetComponent<XManager>();

        float posAxis = bm.transform.position[axis];
        bm.SetHeight(posAxis);
        bm.offset = (posAxis % 2) * (bm.spacing / 2);

        return newObj;
    }

    /// <summary>
    /// Move the platform up or down to the ends of the list depending on the floor direction to move.
    /// </summary>
    /// <param name="direction">The direction the floor should be moved.</param>
    protected override GameObject AfterMoveBlock(GameObject newObj)
    {
        XManager bm = newObj.GetComponent<XManager>();

        float newY = newObj.transform.position[axis];
        bm.SetHeight(newY);

        return newObj;
    }
}
