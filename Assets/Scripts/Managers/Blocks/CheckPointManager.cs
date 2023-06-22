using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [System.Serializable]
    public class CheckPointEvents
    {
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
        public int checkpoint;
        public List<string> events;
    }
    public List<CheckPointEvents> checkpointEventList;

    private Dictionary<int, List<string>> mappedCheckPointEvents;
    private SortedSet<int> checkpointsSet = new SortedSet<int>();
    private int currentCheckPoint = 0;

    void Awake()
    {
        EventManager.GameStart += GameStart;
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

    public void CheckPoint(int axis, Vector3 objPos, CheckPointActions checkpointComp, CheckPointActions opposedReference)
    {
        if (checkpointComp == null) return;

        int? nextCheckPoint = null;
        if (checkpointsSet.Count > 0)
        {
            nextCheckPoint = checkpointsSet.First();
        }

        float objAxisPos = objPos[axis];
        if (nextCheckPoint != null && objAxisPos >= nextCheckPoint)
        {
            currentCheckPoint = (int)nextCheckPoint;
            checkpointsSet.Remove((int)nextCheckPoint);
        }

        if (
            currentCheckPoint == 0 ||
            objAxisPos < currentCheckPoint ||
            mappedCheckPointEvents[currentCheckPoint].Count == 0
            ) return;

        checkpointComp.OnCheckPoint(currentCheckPoint, mappedCheckPointEvents[currentCheckPoint], objPos, opposedReference);
    }

    void GameStart()
    {
        InitiateCheckPointsSet();
    }
}
