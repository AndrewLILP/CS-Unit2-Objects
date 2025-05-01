using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// ScoreManager is responsible for managing the player's score.
/// </summary>

public class ScoreManager : MonoBehaviour
{
    private int seconds;
    private int score;
    private int highScore = 0;


    public UnityEvent OnScoreUpdate;
    public UnityEvent OnHighScoreUpdated;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        OnHighScoreUpdated?.Invoke();
        GameManager.GetInstance().OnGameStart += OnGameStart;
    }

    private void OnGameStart()
    {
        score = 0;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public void SetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public string timer
    {
        get
        {
            return (Mathf.Round((float)seconds / 60.0f) + "mins and" + seconds % 60 + "seconds");
        }
        private set { }
    }

    public int GetScore()
    {
        return score;
    }

    public void IncrementScore()
    {
        score++;
        OnScoreUpdate?.Invoke(); // another way to write a null check - avoids null crash

        if (score > highScore)
        {
            highScore = score;
            OnHighScoreUpdated?.Invoke();
        }
    }

}
