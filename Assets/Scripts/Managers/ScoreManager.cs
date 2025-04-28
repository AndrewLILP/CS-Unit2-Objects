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


    public UnityEvent OnScoreUpdate;

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
    }

}
