using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using TMPro;



public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private TMP_Text txtHighScore;
    [SerializeField] Player player;
    [SerializeField] private GameObject menuCanvas;

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GameManager.GetInstance().scoreManager;
        GameManager.GetInstance().OnGameStart += GameStarted;
        GameManager.GetInstance().OnGameOver += GameOver;
        }

    public void GameStarted()
    {
        UpdateHealth(player.health.GetHealth());
        player.OnHealthUpdate += UpdateHealth; // += subscribes
        menuCanvas.SetActive(false);

    }

    public void GameOver()
    {
        menuCanvas.SetActive(true);
    }

    private void OnDisable()
    {
        //unsubscribe to action
        player.OnHealthUpdate -= UpdateHealth;
    }

    public void UpdateHighScore()
    {
        txtHighScore.SetText(scoreManager.GetHighScore().ToString());
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
