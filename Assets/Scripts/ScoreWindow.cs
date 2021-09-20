using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private Text ScoreText;
    private Text HighScoreText;
    private void Awake()
    {
        ScoreText = transform.Find("Text").GetComponent<Text>();
        HighScoreText = transform.Find("highscoreText").GetComponent<Text>();
    }

    private void Start()
    {
        HighScoreText.text = "HIGHSCORE : " + Score.GetHighScore().ToString();
    }

    private void Update()
    {
        ScoreText.text = Level.GetInstance().getScore().ToString();
    }
}
