using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

// UIManager.cs



public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] Player player;
    // Add missing scoreManager field
    private ScoreManager scoreManager;


    private void Start()
    {
        UpdateHealth(player.health.GetHealth());
        scoreManager = GameManager.GetInstance().scoreManager;

        GameManager.GetInstance().OnGameStart.AddListener(GameStarted);
        GameManager.GetInstance().OnGameOver.AddListener(GameOver);
        
        // Subscribe to score updates
        
        if (scoreManager != null)
        {
            scoreManager.OnScoreUpdate.AddListener(UpdateScore);
        }

    }



    public void GameStarted()
    {
        Debug.Log("GameStarted called!");

        if (player == null)
        {
            Debug.LogError("Player is null in GameStarted!");
            return;
        }

        Debug.Log("Player found, current health: " + player.health.GetHealth());
        UpdateHealth(player.health.GetHealth());
        player.OnHealthUpdate += UpdateHealth; // += subscribes
        Debug.Log("Subscribed to health updates");
    }

    
    private void OnDisable()
    {
        //unsubscribe to action
        if (player != null) 
            player.OnHealthUpdate -= UpdateHealth;

        if (scoreManager != null)
        {
            scoreManager.OnScoreUpdate.RemoveListener(UpdateScore);
        }


        // Unsubscribe from GameManager events
        if (GameManager.GetInstance() != null)
        {
            GameManager.GetInstance().OnGameStart.RemoveListener(GameStarted);
            GameManager.GetInstance().OnGameOver.RemoveListener(GameOver);

        }

        // Unsubscribe from score updates
        if (scoreManager != null)
            scoreManager.OnScoreUpdate.RemoveListener(UpdateScore);
    }

    public void UpdateHealth(float currentHealth)
    {
        Debug.Log("UpdateHealth called with: " + currentHealth);
        txtHealth.SetText(currentHealth.ToString());
    }

    public void UpdateScore()
    {
        txtScore.SetText(GameManager.GetInstance().scoreManager.GetScore().ToString());
    }

    public void GameOver()
    {
        if (player != null)
        {
            player.OnHealthUpdate -= UpdateHealth;
        }
    }
}
