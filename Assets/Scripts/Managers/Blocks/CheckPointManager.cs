using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public void OnCheckPoint(int axis, int checkpoint, List<string> functionsList)
    {
        float objAxisPos = transform.position[axis];
        if (objAxisPos < checkpoint || functionsList.Count == 0) return;

        int childCount = gameObject.transform.childCount;
        List<Task> tasks = new List<Task>();

        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = gameObject.transform.GetChild(i);
            BlockManager blockManager = childTransform.GetComponent<BlockManager>();

            if (blockManager == null) continue;

            blockManager.OnCheckPointEvent(axis, checkpoint, functionsList);
        }
    }
}
