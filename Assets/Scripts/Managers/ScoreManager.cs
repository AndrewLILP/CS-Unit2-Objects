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
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        OnHighScoreUpdated?.Invoke();
        GameManager.GetInstance().OnGameStart += OnGameStart;
    }

    public void OnGameStart()
    {
        score = 0;
        GameManager.GetInstance().uiManager.UpdateHighScore();

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

    public int GetHighScore()
    {
        return highScore;
    }

    // ==================== MODIFIED: INCREMENT SCORE METHOD ====================
    public void IncrementScore()
    {
        score++;
        OnScoreUpdate?.Invoke(); // another way to write a null check - avoids null crash

        // NEW: Update difficulty system when score changes
        GameManager.GetInstance().UpdateDifficulty();

        if (score > highScore)
        {
            SetHighScore();
            OnHighScoreUpdated?.Invoke();
        }

    }
    // ==========================================================================

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
        GameManager.GetInstance().uiManager.UpdateHighScore(); // Update UI with new high score
    }

    // Reset score for new game
    public void ResetScore()
    {
        score = 0;
        OnScoreUpdate?.Invoke();
    }

    // Clean up subscriptions when object is disabled
    private void OnDisable()
    {
        if (GameManager.GetInstance() != null)
        {
            GameManager.GetInstance().OnGameStart -= OnGameStart;
        }
    }

}