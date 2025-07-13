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

    public string timer
    {
        get
        {
            return (Mathf.Round((float)seconds / 60.0f) + "mins and" + seconds % 60 + "seconds");
        }
        private set { }
    }

    // Start method to load high score from PlayerPrefs
    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);

    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highScore;
    }


    public void IncrementScore()
    {
        score++;
        OnScoreUpdate?.Invoke(); // another way to write a null check - avoids null crash
    }

    // Add missing SetHighScore method
    public void SetHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
            Debug.Log("New High Score: " + highScore);
        }
    }

    // Reset score for new game
    public void ResetScore()
    {
        score = 0;
        OnScoreUpdate?.Invoke();
    }

}
