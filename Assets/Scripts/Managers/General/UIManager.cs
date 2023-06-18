using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    TextMeshProUGUI finalScoreText;
    private static GameObject gameOverScreen;
    private static GameObject mainMenuScreen;

    private void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
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
        UpdateScoreUI();
    }

    string ScoreText()
    {
        return GameManager.GetPlayerScore() + " m";
    }

    void GameOver()
    {
        finalScoreText.text = ScoreText();

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

    void UpdateScoreUI()
    {
        scoreText.text = ScoreText();
    }

    public void RestartButton()
    {
        GameManager.FireGameStartEvent();
    }
}
