using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    TextMeshProUGUI finalScoreText;
    TextMeshProUGUI highScoreText;
    private static GameObject gameOverScreen;
    private static GameObject menuScreen;

    private void Awake()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        finalScoreText = GameUtils.FindNestedGameObject("Canvas", "FinalScoreText").GetComponent<TextMeshProUGUI>();
        highScoreText = GameUtils.FindNestedGameObject("Canvas", "HighScoreText").GetComponent<TextMeshProUGUI>();

        gameOverScreen = GameUtils.FindNestedGameObject("Canvas", "GameOverScreen");
        menuScreen = GameUtils.FindNestedGameObject("Canvas", "MainMenuScreen");

        // Subscribe to the events
        EventManager.GameOver += GameOver;
        EventManager.GameStart += GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScoreUI();
    }

    string CurrentScoreText()
    {
        return GameManager.GetPlayerScore() + " m";
    }

    string FinalScoreText()
    {
        return "Score: " + CurrentScoreText();
    }

    string HighScoreText()
    {
        return "High Score: " + GameManager.GetPlayerHighScore() + " m";
    }

    void GameOver()
    {
        finalScoreText.text = FinalScoreText();
        highScoreText.text = HighScoreText();

        gameOverScreen.SetActive(true);
    }

    void GameStart()
    {
        gameOverScreen.SetActive(false);
        menuScreen.SetActive(false);
    }

    void UpdateScoreUI()
    {
        scoreText.text = CurrentScoreText();
    }

    public void RestartButton()
    {
        EventManager.FireGameStartEvent();
    }
}
