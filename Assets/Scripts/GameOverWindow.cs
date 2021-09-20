using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GameOverWindow : MonoBehaviour
{
    private Text Scoretext;
    private Text HighScoretext;

    private void Awake()
    {
        Scoretext = transform.Find("ScoreText").GetComponent<Text>();
        HighScoretext = transform.Find("HighScoreText").GetComponent<Text>();

        transform.Find("RetryBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Loader.Load(Loader.Scene.GameScene);
        };

        transform.Find("MainMenuBtn").GetComponent<Button_UI>().ClickFunc = () => {
            Loader.Load(Loader.Scene.MainMenu);
        };
    }

    private void Start()
    {
        Bird.GetInstance().onDied += Bird_onDied;
        Hide();
    }

    private void Bird_onDied(object sender, System.EventArgs e)
    {
        Scoretext.text = Level.GetInstance().getScore().ToString();
        if(Level.GetInstance().getScore() >= Score.GetHighScore())
        {
            HighScoretext.text = "NEW HIGHSCORE!!";
            Debug.Log("1");
        }
        else
        {
            HighScoretext.text = "HIGHSCORE " + Score.GetHighScore();
        }
        Show();
        SoundManager.PlaySound(SoundManager.Sound.Lose);

    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

}
