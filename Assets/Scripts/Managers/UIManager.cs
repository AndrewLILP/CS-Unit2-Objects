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

        scoreManager = GameManager.GetInstance().scoreManager;
        GameManager.GetInstance().OnGameStart.AddListener(GameStarted);
        GameManager.GetInstance().OnGameOver.AddListener(GameOver);
        // cGPT scoreManager = FindObjectOfType<ScoreManager>();
        // Subscribe to score updates
        scoreManager.OnScoreUpdate.AddListener(UpdateScore);
        // cGPT scoreManager = FindObjectOfType<ScoreManager>();
    }



    public void GameStarted()
    {

        UpdateHealth(player.health.GetHealth());
        player.OnHealthUpdate += UpdateHealth; // += subscribes
    }

    // Add the missing GameOver method
    public void GameOver()
    {
        Debug.Log("Game Over!");
        // Add any game over UI logic here
    }


    private void OnDisable()
    {
        //unsubscribe to action
        if (player != null) 
            player.OnHealthUpdate -= UpdateHealth;


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
        txtHealth.SetText(currentHealth.ToString());
    }

    public void UpdateScore()
    {
        txtScore.SetText(GameManager.GetInstance().scoreManager.GetScore().ToString());
    }
}
