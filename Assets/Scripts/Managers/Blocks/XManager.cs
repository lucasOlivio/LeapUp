using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the creation and movement of blocks horizontally to follow the player X.
/// </summary>
public class XManager : EndlessManager
{
    // public float offset = 0; // Deslocate the platform in the X axis

    // // protected override GameObject AfterCreateBlock(GameObject newBlock)
    // // {
    // //     GameUtils.ChangeSize(newBlock, width, axis);
    // //     GameUtils.ChangePosition(newBlock, transform.position.y, 1);

    // //     return newBlock;
    // // }

    // /// <summary>
    // /// Sets the new height for all of the platform blocks.
    // /// </summary>
    // /// <param name="newHeight">The new height for the platform.</param>
    // public void SetHeight(float newHeight)
    // {
    //     if (tail == null)
    //         return;

    //     Block currentBlock = tail;
    //     float deltaHeight = newHeight - currentBlock.obj.transform.position.y;

    //     if (deltaHeight == 0)
    //         return;

    //     while (currentBlock != null)
    //     {
    //         GameUtils.ChangePosition(currentBlock.obj, deltaHeight, 1);
    //         currentBlock = currentBlock.next;
    //     }
    // }
}
