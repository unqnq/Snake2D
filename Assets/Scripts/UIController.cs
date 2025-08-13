using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public int score = 0;

    private TMP_Text scoreText;

    void Start()
    {
        scoreText = GameObject.Find("ScoreText").GetComponent<TMP_Text>();
    }

    public void UpdateScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
