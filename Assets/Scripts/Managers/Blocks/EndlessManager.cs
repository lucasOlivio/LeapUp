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
    public float offset; // offset in which the player must be
    public GameObject blockPrefab; // Prefab for the block object

    // Definition for events that will happen for each check point
    // Ex:
    // checkpointEventList = [
    //       CheckPointEvents(
    //            checkpoint = 10
    //            events = ["BlockBlink", "MoveBlockX"]
    //         ),
    //         CheckPointEvents(
    //            checkpoint = 20
    //            events = ["MoveBlockY"]
    //         )
    //    ]
    // So when the block is past the 10 Y it will execute the functions "BlockBlink" and "MoveBlockX"
    // When is past the 20 it will stop the other functions and execute only "MoveBlockY"
    [System.Serializable]
    public class CheckPointEvents
    {
        public int checkpoint;
        public List<string> events;
    }
    public List<CheckPointEvents> checkpointEventList;

    private Dictionary<int, List<string>> mappedCheckPointEvents;
    private SortedSet<int> checkpointsSet = new SortedSet<int>();
    private int currentCheckPoint = 0;

    [SerializeField] protected int axis = -1; // In which axis the movement will occur (0 - x; 1 - y; 2 - z)
    private int[] validAxis = { 0, 1, 2 };

    // Inner class for managing each individual block
    protected class Block
    {
        public GameObject obj; // Reference to the block object
        public Block prev; // Reference to the previous block in the list
        public Block next; // Reference to the next block in te list
        public float initialY; // Reset Y

        public Block(GameObject obj, float initialY)
        {
            this.obj = obj;
            this.initialY = initialY;
            this.prev = null;
            this.next = null;
        }
    }

    protected Block head; // Reference to the first block in the list
    protected Block tail; // Reference to the last block in the list

    protected bool created = false; // Check if all the blocks were created
    protected float centerPos; // The center of the area where the player should be

    // Total width for the block including the spacing
    public int BlockTotalWidth()
    {
        return width + spacing;
    }

    void Awake()
    {
        EventManager.GameStart += GameStart;
        EventManager.GameOver += GameOver;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!validAxis.Contains(axis))
            throw new System.InvalidOperationException("Wrong axis!");
    }

    // Update is called once per frame
    void Update()
    {
        if (created && GameManager.isPlayable())
            MoveBlocks();
    }

    void AddCheckPoint(int checkpoint, List<string> eventsList)
    {
        mappedCheckPointEvents[checkpoint] = eventsList;
        checkpointsSet.Add(checkpoint);
    }

    void InitiateCheckPointsSet()
    {
        if (checkpointEventList == null || checkpointEventList.Count == 0) return;

        mappedCheckPointEvents = new Dictionary<int, List<string>> { };
        checkpointsSet = new SortedSet<int> { };
        foreach (CheckPointEvents checkpointEvent in checkpointEventList)
        {
            AddCheckPoint(checkpointEvent.checkpoint, checkpointEvent.events);
        }
    }

    /// <summary>
    /// Inserts a Block object into the linked list.
    /// </summary>
    /// <param name="obj">The new Block to be inserted on the list at the head.</param>
    void InsertOnList(GameObject obj)
    {
        Block floor = new Block(obj, obj.transform.position.y);
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
    /// Creates the initial linked list with the children of the transform.
    /// </summary>
    void CreateInitialBlocks()
    {
        foreach (Transform childTransform in transform)
        {
            InsertOnList(childTransform.gameObject);
        }
        UpdateCenter();
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

    void CheckPoint(GameObject obj)
    {
        CheckPointManager checkpointComp = obj.GetComponent<CheckPointManager>();
        if (checkpointComp == null) return;

        float objAxisPos = obj.transform.position[axis];

        int? nextCheckPoint = null;
        if (checkpointsSet.Count > 0)
        {
            nextCheckPoint = checkpointsSet.First();
        }

        if (nextCheckPoint != null && objAxisPos >= nextCheckPoint)
        {
            currentCheckPoint = (int)nextCheckPoint;
            checkpointsSet.Remove((int)nextCheckPoint);
        }

        if (currentCheckPoint == 0) return;

        checkpointComp.OnCheckPoint(axis, currentCheckPoint, mappedCheckPointEvents[currentCheckPoint]);
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

        // Change the position in the axis accordingly
        float deltaChange = (BlockTotalWidth() * direction) + lastPos[axis] - obj.transform.position[axis];
        GameUtils.ChangePosition(obj, deltaChange, axis);

        CheckPoint(obj);
        UpdateCenter();
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
    /// Moves the platforms up based on the player's position.
    /// </summary>
    void MoveBlocks()
    {
        if (!created)
            return;

        float center = centerPos + offset;
        float currentPlayerPos = GameManager.GetPlayerPosition()[axis];

        if (currentPlayerPos > center)
        {
            GoForwards();
        }
    }

    void GameStart()
    {
        CreateInitialBlocks();
        InitiateCheckPointsSet();
    }

    void GameOver()
    {
        Block current = tail;

        while (current != null)
        {
            GameUtils.SetPosition(current.obj, current.initialY, axis);
            Block next = current.next;
            current.prev = null;
            current.next = null;
            current = next;
        }

        head = null;
        tail = null;
    }
}
