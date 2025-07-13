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
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Game Variables")]
    [SerializeField] private float enemySpawnRate;

    public ScoreManager scoreManager;
    public PickupManager pickupManager;

    /// <summary>
    /// Possible fixes
    /// </summary>
    public UnityEvent OnGameStart;
    public UnityEvent OnGameOver;

    private GameObject tempEnemy;
    private bool isEnemySpawning;

    private bool isPlaying;                                     //   0:32


    private Weapon meleeWeapon = new Weapon("Melee", 1, 0);
    private Weapon exploderWeapon = new Weapon("Exploder", 2, 8); 
    private Weapon machineGunWeapon = new Weapon("Machine Gun", 1, 10);

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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isEnemySpawning = true;
        StartCoroutine(EnemySpawner());
        StartGame();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))    //********** for testing - can delete / comment out
        {
            CreateEnemy();
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

        // Fix deprecated FindObjectsOfType calls
        foreach (Enemy item in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            Destroy(item.gameObject);
        }

        foreach (Pickup item in FindObjectsByType<Pickup>(FindObjectsSortMode.None))
        {
            Destroy(item.gameObject);
        }

    }



    public void NotifyDeath(Enemy enemy)
    {
        pickupManager.SpawnPickup(enemy.transform.position);
    }

    void CreateEnemy()
    {
        GameObject chosenEnemyPrefab = enemyPrefab[UnityEngine.Random.Range(0, enemyPrefab.Length)];
        tempEnemy = Instantiate(chosenEnemyPrefab);
        tempEnemy.transform.position = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;
        //tempEnemy.GetComponent<Enemy>().weapon = meleeWeapon; // works for me as is - Enemy changed to Melee for a fix
        //tempEnemy.GetComponent<MeleeEnemy>().SetMeleeEnemy(2, 0.25f);
        ConfigureEnemy(tempEnemy);
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
    private void ConfigureEnemy(GameObject enemy)
    {
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent == null) return;

        // Configure Melee Enemies
        if (enemy.GetComponent<MeleeEnemy>() != null)
        {
            enemyComponent.weapon = meleeWeapon;
            enemy.GetComponent<MeleeEnemy>().SetMeleeEnemy(2f, 0.25f);
            enemyComponent.SetEnemyType(EnemyType.Melee);
        }
        // Configure Machine Gun Enemies
        else if (enemy.GetComponent<MachineGunEnemy>() != null) // Fixed class name
        {
            var machineGun = enemy.GetComponent<MachineGunEnemy>();

            // Option 1: Use new configuration methods
            machineGun.ConfigureShooter(1f, 8f, 5f, 10f); // damage, range, rate, speed
            machineGun.ConfigureMachineGun(5f, 15f, 3f); // rate, inaccuracy, burst duration

            // Option 2: Use backwards compatible method (comment out Option 1 if using this)
            // machineGun.SetMachineGunEnenmy(5f, 15f);

            enemyComponent.SetEnemyType(EnemyType.MachineGun);
        }
        // Configure Sniper Enemies
        else if (enemy.GetComponent<SniperEnemy>() != null)
        {
            var sniper = enemy.GetComponent<SniperEnemy>();

            // Option 1: Use new configuration methods
            sniper.ConfigureShooter(3f, 12f, 0.33f, 20f); // damage, range, rate, speed
            sniper.ConfigureSniper(1.5f, 1f, 3f); // aim time, inaccuracy, damage

            // Option 2: Use backwards compatible method (comment out Option 1 if using this)
            // sniper.SetSniperEnemy(1.5f, 1f);

            enemyComponent.SetEnemyType(EnemyType.Shooter); // or add EnemyType.Sniper to enum
        }
        // Configure Exploder Enemies
        else if (enemy.GetComponent<ExploderEnemy>() != null)
        {
            // ExploderEnemy doesn't need weapon setup
            enemyComponent.SetEnemyType(EnemyType.Exploder);
        }
        // Add more enemy types here as needed
    }
}
