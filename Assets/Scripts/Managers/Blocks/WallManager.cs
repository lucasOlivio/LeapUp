using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : EndlessManager
{
    [SerializeField] private GameObject cameraObj;
    [SerializeField] private int direction; // Whether the walls should be to left (-1) or right (1)
    private const float CAMERA_WIDTH = 13f;

    private void Awake()
    {
        axis = 1;
    }

    protected override GameObject AfterCreateBlock(GameObject newBlock)
    {
        float delta = newBlock.transform.position.x - (cameraObj.transform.position.x + (direction * CAMERA_WIDTH));
        GameUtils.ChangePosition(newBlock, delta, 0);
        return newBlock;
    }
}
