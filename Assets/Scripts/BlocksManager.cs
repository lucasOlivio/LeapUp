using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the creation and movement of blocks horizontally to follow the player X.
/// </summary>
public class BlocksManager : EndlessManager
{
    public float offset = 0; // Deslocate the platform in the X axis

    private void Awake()
    {
        axis = 0;
    }

    protected override GameObject CreateBlock(float posAxis) {
        GameObject newObj = base.CreateBlock(posAxis);
        GameUtils.ChangePosition(newObj, transform.position.y, 1);

        return newObj;
    }

    protected override float getPosForBlock(int nBlock) {
        return base.getPosForBlock(nBlock) + offset;
    }

    /// <summary>
    /// Sets the height for the platform blocks and moves them to the new height.
    /// </summary>
    /// <param name="newHeight">The new height for the platform.</param>
    public void SetHeight(float newHeight) {
        if (tail == null)
            return;

        float deltaHeight = newHeight - tail.obj.transform.position.y;

        if (deltaHeight == 0)
            return;

        Block currentBlock = tail;

        while (currentBlock != null)
        {
            GameUtils.ChangePosition(currentBlock.obj, deltaHeight, 1);
            currentBlock = currentBlock.next;
        }
    }

}
