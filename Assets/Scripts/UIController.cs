using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    Player player;
    TextMeshProUGUI heightText;
    TextMeshProUGUI finalScoreText;
    private static GameObject gameOverScreen;
    private static GameObject mainMenuScreen;

    private void Awake() {
        player = GameObject.Find("Player").GetComponent<Player>();
        heightText = GameObject.Find("HeightText").GetComponent<TextMeshProUGUI>();
        finalScoreText = GameUtils.FindNestedGameObject("Canvas", "FinalScoreText").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {   
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
        finalScoreText.text = "Final score: " + player.highScore + " m";

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

    void UpdateHighScore() {
        heightText.text = player.highScore + " m";
    }

    public void RestartButton() {
        GameManager.FireGameStartEvent();
    }
}
