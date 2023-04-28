using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameObject Player { get; private set; }

    // Define game events delegates
    public delegate void GameStartDelegate();
    public delegate void PlayerStartDelegate();
    public delegate void GameOverDelegate();

    // Define the events
    public static event GameStartDelegate GameStart;
    public static event PlayerStartDelegate PlayerStart;
    public static event GameOverDelegate GameOver;

    public enum GameStates
    {
        MainMenu,       // The game is in the main menu
        Playing,        // The game is actively being played
        Paused,         // The game is paused
        GameOver        // The game is over
    }
    public static List<GameStates> playable = new List<GameStates> { GameStates.MainMenu, GameStates.Playing };
    private static GameStates _state;

    // Public getter for the state variable
    public static GameStates state
    {
        get { return _state; }
    }

    private void Awake()
    {
        Player = GameObject.Find("Player");

        if (Player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }

        GameStartSetup();
    }

    // Methods to fire the respectives events
    public static void FireGameStartEvent()
    {
        // Check if there are any subscribers to the event
        if (GameStart != null)
        {
            // Fire the event, which will notify all subscribers
            GameStart();
        }
        GameStartSetup();
    }

    public static void FirePlayerStartEvent()
    {
        // Check if there are any subscribers to the event
        if (PlayerStart != null)
        {
            // Fire the event, which will notify all subscribers
            PlayerStart();
        }
        PlayerStartSetup();
    }

    public static void FireGameOverEvent()
    {
        // Check if there are any subscribers to the event
        if (GameOver != null)
        {
            // Fire the event, which will notify all subscribers
            GameOver();
        }
        GameOverSetup();
    }

    // Check if the actual state of the game is playable
    public static bool isPlayable() {
        return playable.Contains(_state);
    }

    private static void GameStartSetup() {
        _state = GameStates.MainMenu;
    }

    private static void PlayerStartSetup() {
        _state = GameStates.Playing;
    }

    private static void GameOverSetup() {
        _state = GameStates.GameOver;
    }
}

