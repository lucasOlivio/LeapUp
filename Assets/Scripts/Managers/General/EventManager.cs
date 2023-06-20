using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // Define game events delegates
    public delegate void GameStartDelegate();
    public delegate void PlayerStartDelegate();
    public delegate void GameOverDelegate();

    // Define the events
    public static event GameStartDelegate GameStart;
    public static event PlayerStartDelegate PlayerStart;
    public static event GameOverDelegate GameOver;

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
        // Check if there are any subscribers to the event
        if (PlayerStart != null)
        {
            // Fire the event, which will notify all subscribers
            PlayerStart();
        }
    }

    public static void FireGameOverEvent()
    {
        // Check if there are any subscribers to the event
        if (GameOver != null)
        {
            // Fire the event, which will notify all subscribers
            GameOver();
        }
    }
}
