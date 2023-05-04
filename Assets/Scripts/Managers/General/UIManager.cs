using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    TextMeshProUGUI heightText;
    PlayerController playerController;

    TextMeshProUGUI finalScoreText;
    private static GameObject gameOverScreen;
    private static GameObject mainMenuScreen;

    private void Awake()
    {
        heightText = GameObject.Find("HeightText").GetComponent<TextMeshProUGUI>();
        finalScoreText = GameUtils.FindNestedGameObject("Canvas", "FinalScoreText").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameManager.Player.GetComponent<PlayerController>();
        gameOverScreen = GameUtils.FindNestedGameObject("Canvas", "GameOverScreen");
        mainMenuScreen = GameUtils.FindNestedGameObject("Canvas", "MainMenuScreen");

        // Subscribe to the events
        GameManager.GameOver += GameOver;
        GameManager.PlayerStart += PlayerStart;
        GameManager.GameStart += GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHighScore();
    }

    void GameOver()
    {
        finalScoreText.text = playerController.highScore + " m";

        gameOverScreen.SetActive(true);
    }

    void GameStart()
    {
        gameOverScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    void PlayerStart()
    {
        mainMenuScreen.SetActive(false);
    }

    void UpdateHighScore()
    {
        heightText.text = playerController.highScore + " m";
    }

    public void RestartButton()
    {
        GameManager.FireGameStartEvent();
    }
}
