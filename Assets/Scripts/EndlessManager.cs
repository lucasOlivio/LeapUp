using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to manage endless movement for platforms and blocks, horizontally or vertically.
/// </summary>
public class EndlessManager : MonoBehaviour
{
    public int spacing; // Space between the blocks
    public int nBlocks; // Number of blocks total
    public GameObject blockPrefab; // Prefab for the block object

    // Inner class for managing each individual block
    protected class Block {
        public GameObject obj; // Reference to the block object
        public Block prev; // Reference to the previous block in the list
        public Block next; // Reference to the next block in te list

        public Block(GameObject obj) {
            this.obj = obj;
            this.prev = null;
            this.next = null;
        }
    }

    protected Block head; // Reference to the first block in the list
    protected Block tail; // Reference to the last block in the list
    protected bool created = false; // Check if all the blocks were created
    protected int axis = -1; // In which axis the movement will occur (0 - x; 1 - y; 2 - z)
    protected float currentPlayerPos; // Current player position
    protected float center; // The center of the area where the player should be
    [SerializeField] protected int width; // The width for each unit of block

    // Start is called before the first frame update
    void Start()
    {
        if (axis != 0 && axis != 1 && axis != 2)
            throw new System.InvalidOperationException("Wrong axis!");

        head = null;
        tail = null;
        center =  ((nBlocks) * (width + spacing)) / 2;

        CreateInitialBlocks();
    }

    // Update is called once per frame
    void Update()
    {
        if(created && GameManager.isPlayable())
            MoveBlocks();
    }

    /// <summary>
    /// Calculates the position for the platform at the specified index.
    /// </summary>
    /// <param name="nBlock">The index of the block.</param>
    /// <returns>The "axis" position for the platform.</returns>
    protected virtual float getPosForBlock(int nBlock) {
        int playerPosition = (int)GameManager.Player.transform.position[axis];

        return playerPosition + (nBlock * spacing) - center + (spacing / 2);
    }

    /// <summary>
    /// Instantiates a block object at the specified position.
    /// </summary>
    /// <param name="position">The position the new block will be in the given axis.</param>
    /// <returns>The new platformManager object.</returns>
    protected virtual GameObject CreateBlock(float position) {
        GameObject newBlock = Instantiate(blockPrefab);

        // Dinamically changes the block width and positioning
        GameUtils.ChangeScale(newBlock, width, axis);
        GameUtils.ChangePosition(newBlock, position, axis);

        return newBlock;
    }

    /// <summary>
    /// Inserts a Block object into the linked list.
    /// </summary>
    /// <param name="obj">The new Block to be inserted on the list at the head.</param>
    void Insert(GameObject obj) {
        Block floor = new Block(obj);
        if (head == null) {
            tail = floor;
        } else {
            head.next = floor;
            floor.prev = head;
        }
        head = floor;
    }

    /// <summary>
    /// Creates the initial blocks by instantiating gameobjects with its respective "axis"
    /// and inserting them into the linked list.
    /// </summary>
    void CreateInitialBlocks() {
        for(int nBlock = 1; nBlock <= nBlocks; nBlock++) {
            float posAxis = getPosForBlock(nBlock);
            GameObject pm = CreateBlock(posAxis);
            Insert(pm);
        }
        center = GameManager.Player.transform.position[axis];
        created = true;
    }

    /// <summary>
    /// Update the center of the blocks, using the head and tail as reference
    /// </summary>
    void UpdateCenter(int direction) {
        Vector3 headPosition = head.obj.transform.position;
        Vector3 tailPosition = tail.obj.transform.position;
        float middle = System.Math.Abs(headPosition[axis] - tailPosition[axis]) / 2;
        
        center = middle + tailPosition[axis];
    }

    /// <summary>
    /// Move the block, up or down, left or right, to the ends of the list depending on the direction to move.
    /// </summary>
    /// <param name="direction">The direction the block should be moved.</param>
    protected virtual GameObject MoveBlock(int direction) {
        GameObject obj = null;
        Vector3 lastPos;

        if(direction == 1) {
            obj = head.obj;
            lastPos = head.prev.obj.transform.position;
        } else if (direction == -1) {
            obj = tail.obj;
            lastPos = tail.next.obj.transform.position;
        } else {
            throw new System.Exception("Wrong direction, must be 1 or -1!");
        }

        float deltaChange = (spacing * direction) + lastPos[axis] - obj.transform.position[axis];
        GameUtils.ChangePosition(obj, deltaChange, axis);
        UpdateCenter(direction);

        return obj;
    }

    /// <summary>
    /// Removes the block from the tail of the list and set as the head.
    /// </summary>
    void GoForwards() {
        if (head == null || head == tail) {
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
    void GoBackwards() {
        if (tail == null || head == tail) {
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
    void MoveBlocks() {
        currentPlayerPos = (int)GameManager.Player.transform.position[axis];
        float area = width * 2; // Area in which the player must be

        if(head == null)
            return;

        if(currentPlayerPos < center - area)
            GoBackwards();
        else if (currentPlayerPos > center + area)
            GoForwards();
    }
}
