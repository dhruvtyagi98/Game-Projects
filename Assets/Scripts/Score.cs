using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Score
{
    public static void start()
    {
        Bird.GetInstance().onDied += bird_onDied;
    }

    private static void bird_onDied(object sender, System.EventArgs e)
    {
        SetHighScore(Level.GetInstance().getScore());
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore");
    }

    public static bool SetHighScore(int score)
    {
        int currentHighScore = GetHighScore();
        if(score > currentHighScore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void resethighscore()
    {
        PlayerPrefs.SetInt("highscore", 0);
        PlayerPrefs.Save();
    }
}
