using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        MainMenu,       // The game is in the main menu
        GameStart,      // The game started
        Playing,        // The game is actively being played
        Paused,         // The game is paused
        GameOver        // The game is over
    }
    public static List<GameStates> playable = new List<GameStates> { GameStates.GameStart, GameStates.Playing };
    private static GameStates _state;
    private static GameObject Player;

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

        EventManager.GameStart += GameStartSetup;
        EventManager.PlayerStart += PlayerStartSetup;
        EventManager.GameOver += GameOverSetup;

        _state = GameStates.MainMenu;
    }

    // Returns the current player position
    public static Vector3 GetPlayerPosition()
    {
        return Player.transform.position;
    }

    // Return the players current score
    public static int GetPlayerScore()
    {
        return Player.GetComponent<PlayerController>().score;
    }



    // Check if the actual state of the game is playable
    public static bool isPlayable()
    {
        return playable.Contains(_state);
    }

    private static void GameStartSetup()
    {
        _state = GameStates.GameStart;
    }

    private static void PlayerStartSetup()
    {
        _state = GameStates.Playing;
    }

    private static void GameOverSetup()
    {
        _state = GameStates.GameOver;
    }
}

