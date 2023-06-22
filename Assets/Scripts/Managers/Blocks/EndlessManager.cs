using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A class to manage endless movement for platforms and nodes, horizontally or vertically.
/// </summary>
public class EndlessManager : MonoBehaviour
{
    public int spacing; // Space between the nodes
    public int width; // The width for each unit of node
    public float offset; // offset in which the player must be

    [SerializeField] private int axis = -1; // In which axis the movement will occur (0 - x; 1 - y; 2 - z)
    private int[] validAxis = { 0, 1, 2 };

    // Inner class for managing each individual Node
    private class Node
    {
        public GameObject obj; // Reference to the Node object
        public Node prev; // Reference to the previous Node in the list
        public Node next; // Reference to the next Node in te list
        public float initialAxisPos; // Reset Axis position
        public Vector3 position; // Reference position

        public Node(GameObject obj, float initialAxisPos, Vector3 position)
        {
            this.obj = obj;
            this.initialAxisPos = initialAxisPos;
            this.position = position;
            this.prev = null;
            this.next = null;
        }
    }

    private Node head; // Reference to the first Node in the list
    private Node tail; // Reference to the last Node in the list

    private bool created = false; // Check if all the Nodes were created
    private float centerPos; // The center of the area where the player should be

    private CheckPointManager checkpointManager;

    // Total width for the node including the spacing
    public int NodeTotalWidth()
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

        GameManager gameManager = FindObjectOfType<GameManager>();
        checkpointManager = gameManager.GetComponent<CheckPointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (created && GameManager.isPlayable())
            MoveNodes();
    }

    /// <summary>
    /// Inserts a Node object into the linked list.
    /// </summary>
    /// <param name="obj">The new Node to be inserted on the list at the head.</param>
    void InsertOnList(GameObject obj)
    {
        Node node = new Node(obj, obj.transform.position[axis], obj.transform.position);
        if (head == null)
        {
            tail = node;
        }
        else
        {
            head.next = node;
            node.prev = head;
        }
        head = node;
    }

    /// <summary>
    /// Creates the initial linked list with the children of the transform.
    /// </summary>
    void CreateInitialNodes()
    {
        foreach (Transform childTransform in transform)
        {
            InsertOnList(childTransform.gameObject);
        }
        UpdateCenter();
        created = true;
    }

    /// <summary>
    /// Update the center of the nodes, using the head and tail as reference.
    /// For node move can't use the player position, because it can be too uncertain
    /// </summary>
    void UpdateCenter()
    {
        Vector3 headPosition = head.position;
        Vector3 tailPosition = tail.position;
        float middle = System.Math.Abs(headPosition[axis] - tailPosition[axis]) / 2;

        centerPos = middle + tailPosition[axis];
    }

    /// <summary>
    /// Move the node, up or down, left or right, to the ends of the list depending on the direction to move.
    /// </summary>
    /// <param name="direction">The direction the node should be moved.</param>
    protected virtual void MoveNode(int direction)
    {
        Node node;
        Node lastNode;
        GameObject obj;
        float objAxisPos;
        Vector3 lastPos;

        // Get the back end of the list depending on the direction player is going
        if (direction == 1)
        {
            node = head;
            lastNode = head.prev;
            lastPos = head.prev.position;
        }
        else if (direction == -1)
        {
            node = tail;
            lastNode = tail.next;
        }
        else
        {
            throw new System.Exception("Wrong direction, must be 1 or -1!");
        }

        obj = node.obj;
        objAxisPos = node.position[axis];
        lastPos = lastNode.position;

        // Change the position in the axis accordingly
        float deltaChange = (NodeTotalWidth() * direction) + lastPos[axis] - objAxisPos;
        node.position[axis] = GameUtils.ChangePosition(obj, deltaChange, axis);

        CheckPointActions checkpointActions = obj.GetComponent<CheckPointActions>();
        CheckPointActions opposedReference = lastNode.obj.GetComponent<CheckPointActions>();
        checkpointManager.CheckPoint(axis, node.position, checkpointActions, opposedReference);
        UpdateCenter();
    }

    /// <summary>
    /// Removes the node from the tail of the list and set as the head.
    /// </summary>
    void GoForwards()
    {
        if (head == null || head == tail)
        {
            return;
        }
        Node node = tail;
        tail = tail.next;
        tail.prev = null;
        node.next = null;
        node.prev = head;
        head.next = node;
        head = node;

        MoveNode(1);
    }

    /// <summary>
    /// Moves the platforms up based on the player's position.
    /// </summary>
    void MoveNodes()
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
        CreateInitialNodes();
    }

    void GameOver()
    {
        Node current = tail;

        while (current != null)
        {
            GameUtils.SetPosition(current.obj, current.initialAxisPos, axis);
            Node next = current.next;
            current.prev = null;
            current.next = null;
            current = next;
        }

        head = null;
        tail = null;
    }
}
