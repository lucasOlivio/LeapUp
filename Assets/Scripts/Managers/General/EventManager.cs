using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Define game events delegates
    public delegate void GameStartDelegate();
    public delegate void PlayerStartDelegate();
    public delegate void GameOverDelegate();
    public delegate void CheckPointDelegate(int axis, int currentCheckpoint, List<string> eventsList);

    // Define the events
    public static event GameStartDelegate GameStart;
    public static event PlayerStartDelegate PlayerStart;
    public static event GameOverDelegate GameOver;
    public static event CheckPointDelegate OnCheckPointEvent;

    // Methods to fire the respectives events
    public static void FireGameStartEvent()
    {
        // Check if there are any subscribers to the event
        if (GameStart != null)
        {
            // Fire the event, which will notify all subscribers
            GameStart();
        }
    }

    public static void FirePlayerStartEvent()
    {
        if (PlayerStart != null)
        {
            PlayerStart();
        }
    }

    public static void FireGameOverEvent()
    {
        if (GameOver != null)
        {
            GameOver();
        }
    }


    public static void FireCheckPointEvent(int axis, int currentCheckpoint, List<string> eventsList)
    {
        if (OnCheckPointEvent != null)
        {
            OnCheckPointEvent(axis, currentCheckpoint, eventsList);
        }
    }
}
