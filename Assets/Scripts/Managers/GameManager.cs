using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

// GameManager.cs



/// <summary>
/// GameManager is responsible for managing the game state and flow.
/// </summary>

public class GameManager : MonoBehaviour
{
    [Header("Game Entities")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Game Variables")]
    [SerializeField] private float enemySpawnRate;

    public Action OnGameStart;
    public Action OnGameOver;

    public ScoreManager scoreManager;
    public PickupManager pickupManager;
    public UIManager uiManager;

    private GameObject tempEnemy;
    private bool isEnemySpawning;
    private bool isPlaying;                                     //   0:32

    private Weapon meleeWeapon = new Weapon("Melee", 1, 0);

    /// <summary>
    /// Singleton
    /// it is a design pattern that allows a class to have only one instance and provides a global point of access to it.
    /// other gameManagers would be deleted
    /// </summary>
    /// 
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    void SetSingleton()
    {
        if (instance != null && instance != this)
        {
            //Destroy(this);
            Destroy(this.gameObject);
        }
        instance = this;
    }

    private void Awake()
    {
        // Set the singleton instance
        SetSingleton();

    }
    // end of singleton


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))    //********** for testing - can delete / comment out
        {
            CreateEnemy();
        }
        // CLAUDE FIX
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartGame();
        }
    }

    public Player GetPlayer()
    {
        return player;

    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    public void StartGame()
    {
        player.gameObject.SetActive(true);
        player.OnDeath += StopGame; // += subscribing to the action
        isPlaying = true;
        OnGameStart?.Invoke();
        StartCoroutine(GameStarter());
    }

    IEnumerator GameStarter()
    {
        yield return new WaitForSeconds(2.0f);
        isEnemySpawning = true;
        StartCoroutine(EnemySpawner());
    }

    public void StopGame()
    {
        isEnemySpawning = false;
        scoreManager.SetHighScore();
        StartCoroutine(GameStopper());
    }

    IEnumerator GameStopper()
    {
        isEnemySpawning = false;
        yield return new WaitForSeconds(2.0f);
        isPlaying = false;

        foreach (Enemy item in FindObjectsOfType(typeof(Enemy)))
        {
            Destroy(item.gameObject);
        }

        foreach (Pickup item in FindObjectsOfType(typeof(Enemy)))
        {
            Destroy(item.gameObject);
        }

        OnGameOver?.Invoke();
    
    }



    public void NotifyDeath(Enemy enemy)
    {
        pickupManager.SpawnPickup(enemy.transform.position);
    }

    void CreateEnemy()
    {
        tempEnemy = Instantiate(enemyPrefab);
        tempEnemy.transform.position = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;
        tempEnemy.GetComponent<Enemy>().weapon = meleeWeapon; // works for me as is - Enemy changed to Melee for a fix
        tempEnemy.GetComponent<MeleeEnemy>().SetMeleeEnemy(2, 0.25f);
    }

    IEnumerator EnemySpawner()
    {
        while (isEnemySpawning)
        {
            // run things before the wait
            yield return new WaitForSeconds(1.0f / enemySpawnRate);
            // run things after the wait
            CreateEnemy();
        }
    }
}
