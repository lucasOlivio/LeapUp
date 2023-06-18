using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A class to manage endless movement for platforms and blocks, horizontally or vertically.
/// </summary>
public class EndlessManager : MonoBehaviour
{
    public int spacing; // Space between the blocks
    public int width; // The width for each unit of block
    public float area; // Area in which the player must be
    public GameObject blockPrefab; // Prefab for the block object

    [SerializeField] protected int axis = -1; // In which axis the movement will occur (0 - x; 1 - y; 2 - z)
    private int[] validAxis = { 0, 1, 2 };

    // Inner class for managing each individual block
    protected class Block
    {
        public GameObject obj; // Reference to the block object
        public Block prev; // Reference to the previous block in the list
        public Block next; // Reference to the next block in te list

        public Block(GameObject obj)
        {
            this.obj = obj;
            this.prev = null;
            this.next = null;
        }
    }

    protected Block head; // Reference to the first block in the list
    protected Block tail; // Reference to the last block in the list

    protected bool created = false; // Check if all the blocks were created
    protected int currentPlayerPos; // Current player position
    protected float centerPos; // The center of the area where the player should be

    // Total width for the block including the spacing
    public int BlockTotalWidth()
    {
        return width + spacing;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!validAxis.Contains(axis))
            throw new System.InvalidOperationException("Wrong axis!");

        UpdatePlayerCurrentAxisPosition();
        CreateInitialBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerCurrentAxisPosition();
        if (created && GameManager.isPlayable())
            MoveBlocks();
    }

    // Set the player current position in "axis"
    protected void UpdatePlayerCurrentAxisPosition()
    {
        currentPlayerPos = (int)GameManager.GetPlayerPosition()[axis];
    }

    /// <summary>
    /// Inserts a Block object into the linked list.
    /// </summary>
    /// <param name="obj">The new Block to be inserted on the list at the head.</param>
    void InsertOnList(GameObject obj)
    {
        Block floor = new Block(obj);
        if (head == null)
        {
            tail = floor;
        }
        else
        {
            head.next = floor;
            floor.prev = head;
        }
        head = floor;
    }

    /// <summary>
    /// Creates the initial blocks by instantiating gameobjects with its respective "axis".
    /// </summary>
    void CreateInitialBlocks()
    {
        Transform[] children = GameUtils.GetOrderedChildren(transform);
        for (int i = 0; i < children.Length; i++)
        {
            InsertOnList(children[i].gameObject);
        }
        created = true;
    }

    /// <summary>
    /// Update the center of the blocks, using the head and tail as reference.
    /// For block move can't use the player position, because it can be too uncertain
    /// </summary>
    void UpdateCenter()
    {
        Vector3 headPosition = head.obj.transform.position;
        Vector3 tailPosition = tail.obj.transform.position;
        float middle = System.Math.Abs(headPosition[axis] - tailPosition[axis]) / 2;

        centerPos = middle + tailPosition[axis];
    }

    /// <summary>
    /// Move the block, up or down, left or right, to the ends of the list depending on the direction to move.
    /// </summary>
    /// <param name="direction">The direction the block should be moved.</param>
    protected virtual void MoveBlock(int direction)
    {
        GameObject obj = null;
        Vector3 lastPos;

        // Get the back end of the list depending on the direction player is going
        if (direction == 1)
        {
            obj = head.obj;
            lastPos = head.prev.obj.transform.position;
        }
        else if (direction == -1)
        {
            obj = tail.obj;
            lastPos = tail.next.obj.transform.position;
        }
        else
        {
            throw new System.Exception("Wrong direction, must be 1 or -1!");
        }

        Debug.Log($"OBJ POS {axis}: {obj.transform.position[axis]}");
        Debug.Log($"LASTOBJ POS {axis}: {lastPos[axis]}");
        Debug.Log($"CALC POS {axis}: {(BlockTotalWidth() * direction) + lastPos[axis] - obj.transform.position[axis]}");

        // Change the position in the axis accordingly
        float deltaChange = (BlockTotalWidth() * direction) + lastPos[axis] - obj.transform.position[axis];
        GameUtils.ChangePosition(obj, deltaChange, axis);
        UpdateCenter();

        // Update the Block initial position
        BlockManager bm = obj.GetComponent<BlockManager>();
        if (bm != null)
        {
            bm.UpdateInitialPosition();
        }
    }

    /// <summary>
    /// Removes the block from the tail of the list and set as the head.
    /// </summary>
    void GoForwards()
    {
        if (head == null || head == tail)
        {
            return;
        }
        Block block = tail;
        tail = tail.next;
        tail.prev = null;
        block.next = null;
        block.prev = head;
        head.next = block;
        head = block;

        MoveBlock(1);
    }

    /// <summary>
    /// Removes the block from the head of the list and set as the tail.
    /// </summary>
    void GoBackwards()
    {
        if (tail == null || head == tail)
        {
            return;
        }
        Block block = head;
        head = head.prev;
        head.next = null;
        block.prev = null;
        block.next = tail;
        tail.prev = block;
        tail = block;

        MoveBlock(-1);
    }

    /// <summary>
    /// Moves the platforms up or down based on the player's position.
    /// </summary>
    void MoveBlocks()
    {
        if (!created)
            return;

        Debug.Log($"{axis} MOVING FORWARD");
        if (currentPlayerPos < centerPos - area)
        {
            Debug.Log($"{axis} MOVING BACKWARDS");
            GoBackwards();
        }
        else if (currentPlayerPos > centerPos + area)
        {
            Debug.Log($"{axis} MOVING FORWARD");
            GoForwards();
        }
    }
}
