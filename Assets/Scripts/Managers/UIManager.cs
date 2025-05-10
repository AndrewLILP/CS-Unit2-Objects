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

    private void Start()
    {
<<<<<<< HEAD
        scoreManager = GameManager.GetInstance().scoreManager;
        GameManager.GetInstance().OnGameStart += GameStarted;
        GameManager.GetInstance().OnGameOver += GameOver;
        }

    // ********************* Claude fix



    public void GameStarted()
    {
=======
>>>>>>> parent of c43466d (lesson009-April24)
        UpdateHealth(player.health.GetHealth());
        player.OnHealthUpdate += UpdateHealth; // += subscribes
    }

    private void OnDisable()
    {
        //unsubscribe to action
        player.OnHealthUpdate -= UpdateHealth;
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
