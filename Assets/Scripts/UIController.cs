using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public int score = 0;

    private TMP_Text scoreText;
    private GameObject gameOverPanel;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
        gameOverPanel = GameObject.Find("GameOverPanel");
        gameOverPanel.SetActive(false);
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        if (score > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
            Debug.Log("best score: " + PlayerPrefs.GetInt("BestScore"));
        }
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void OpenMenu()
    {
        if (score > PlayerPrefs.GetInt("BestScore", 0))
        {
            PlayerPrefs.SetInt("BestScore", score);
            Debug.Log("best score: " + score);
        }
        SceneManager.LoadScene("Menu");
    }
}
