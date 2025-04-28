using UnityEngine;

using System.Collections.Generic;
using System.Collections;
using TMPro;



public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] Player player;

    private void Start()
    {
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
