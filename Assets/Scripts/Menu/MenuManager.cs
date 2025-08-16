using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private GameObject optionsPanel;
    private TMP_Text bestScore;

    void Start()
    {
        optionsPanel = GameObject.Find("OptionsPanel");
        bestScore = GameObject.Find("BestScoreText").GetComponent<TMP_Text>();
        bestScore.text = "Best score: " + PlayerPrefs.GetInt("BestScore", 0);
    }
    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
