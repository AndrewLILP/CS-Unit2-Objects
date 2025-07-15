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

    // ==================== NEW: DIFFICULTY SYSTEM ====================
    [Header("Difficulty System")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int pointsPerLevel = 20;
    // =================================================================

    // ==================== NEW: SPAWN PATTERN SYSTEM ====================
    [Header("Spawn Pattern System")]
    [SerializeField] private SpawnPatternSystem spawnPatternSystem;
    // ====================================================================

    public ScoreManager scoreManager;
    public PickupManager pickupManager;
    public UIManager uiManager; // Added UIManager reference

    /// <summary>
    /// Possible fixes
    /// </summary>
    public Action OnGameStart; // changed in class10
    public Action OnGameOver; // changed in class10

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

        // ==================== NEW: INITIALIZE SPAWN PATTERN SYSTEM ====================
        if (spawnPatternSystem == null)
        {
            spawnPatternSystem = GetComponent<SpawnPatternSystem>();
            if (spawnPatternSystem == null)
            {
                spawnPatternSystem = gameObject.AddComponent<SpawnPatternSystem>();
            }
        }
        // ==============================================================================
    }
    // end of singleton



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

        // ==================== NEW: RESET DIFFICULTY ON GAME START ====================
        currentLevel = 1;
        enemySpawnRate = GetSpawnRateForLevel(); // Set initial spawn rate
                                                 // ==============================================================================

        OnGameStart?.Invoke(); // short hand for null check
        StartCoroutine(GameStarter());
    }

    IEnumerator GameStarter()
    {
        yield return new WaitForSeconds(2.0f);
        if (player.health.GetHealth() <= 0)
        {
            player.health.AddHealth(100); // Ensure player starts with full health
        }
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

    // ==================== NEW: DIFFICULTY MANAGEMENT METHODS ====================
    public void UpdateDifficulty()
    {
        int newLevel = (scoreManager.GetScore() / pointsPerLevel) + 1;
        if (newLevel > currentLevel)
        {
            currentLevel = newLevel;
            Debug.Log($"Level Up! Now on Level {currentLevel}");
            // Update spawn rate for new level
            enemySpawnRate = GetSpawnRateForLevel();
        }
    }

    private float GetSpawnRateForLevel()
    {
        switch (currentLevel)
        {
            case 1: return 0.5f;  // Easy introduction
            case 2: return 0.7f;  // Slight increase
            case 3: return 1.0f;  // Moderate
            case 4: return 1.2f;  // Getting tough
            default:
                // Progressive increase but capped at 2.0 to prevent chaos
                return Mathf.Min(2.0f, 1.2f + (currentLevel - 4) * 0.1f);
        }
    }

    private float[] GetEnemyWeights()
    {
        switch (currentLevel)
        {
            case 1:
                return new float[] { 1f, 0f, 0f, 0f }; // Melee only
            case 2:
                return new float[] { 0.6f, 0.4f, 0f, 0f }; // Melee + Exploder
            case 3:
                return new float[] { 0.4f, 0.3f, 0.3f, 0f }; // + Shooter
            case 4:
                return new float[] { 0.3f, 0.2f, 0.3f, 0.2f }; // + Machine Gun (nerfed)
            default:
                return new float[] { 0.25f, 0.25f, 0.25f, 0.25f }; // Balanced chaos
        }
    }

    private GameObject GetEnemyForCurrentLevel()
    {
        float[] weights = GetEnemyWeights();
        float totalWeight = 0f;

        // Calculate total weight for available enemies
        for (int i = 0; i < weights.Length && i < enemyPrefab.Length; i++)
            totalWeight += weights[i];

        if (totalWeight <= 0f) return enemyPrefab[0]; // Safety fallback

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        // Select enemy based on weighted probability
        for (int i = 0; i < weights.Length && i < enemyPrefab.Length; i++)
        {
            currentWeight += weights[i];
            if (randomValue <= currentWeight)
                return enemyPrefab[i];
        }

        return enemyPrefab[0]; // Fallback to first enemy
    }
    // ============================================================================

    // ==================== MODIFIED: CREATEENEMY METHOD ====================
    void CreateEnemy()
    {
        // NEW: Check if we should use a spawn pattern instead of single enemy
        if (spawnPatternSystem != null && spawnPatternSystem.ShouldUsePattern(currentLevel))
        {
            spawnPatternSystem.ExecuteRandomPattern(currentLevel);
            return; // Pattern will handle spawning
        }

        // Original single enemy spawn logic
        GameObject chosenEnemyPrefab = GetEnemyForCurrentLevel();
        tempEnemy = Instantiate(chosenEnemyPrefab);
        tempEnemy.transform.position = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;

        ConfigureEnemy(tempEnemy);
    }
    // =======================================================================

    // ==================== NEW: METHODS FOR SPAWN PATTERN SYSTEM ====================
    public void SpawnSpecificEnemyAtPoint(EnemyType enemyType, int spawnPointIndex)
    {
        GameObject enemyPrefab = GetEnemyPrefabByType(enemyType);
        if (enemyPrefab == null)
        {
            Debug.LogError($"No prefab found for enemy type: {enemyType}");
            return;
        }

        // Ensure spawn point index is valid
        if (spawnPointIndex < 0 || spawnPointIndex >= spawnPoints.Length)
        {
            spawnPointIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
            Debug.LogWarning($"Invalid spawn point index, using random: {spawnPointIndex}");
        }

        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.position = spawnPoints[spawnPointIndex].position;
        ConfigureEnemy(enemy);

        Debug.Log($"Spawned {enemyType} at spawn point {spawnPointIndex}");
    }

    private GameObject GetEnemyPrefabByType(EnemyType enemyType)
    {
        // This assumes your enemyPrefab array is ordered as:
        // [0] = Melee, [1] = Exploder, [2] = Shooter, [3] = MachineGun
        switch (enemyType)
        {
            case EnemyType.Melee:
                return enemyPrefab.Length > 0 ? enemyPrefab[0] : null;
            case EnemyType.Exploder:
                return enemyPrefab.Length > 1 ? enemyPrefab[1] : null;
            case EnemyType.Shooter:
                return enemyPrefab.Length > 2 ? enemyPrefab[2] : null;
            case EnemyType.MachineGun:
                return enemyPrefab.Length > 3 ? enemyPrefab[3] : null;
            default:
                return enemyPrefab.Length > 0 ? enemyPrefab[0] : null;
        }
    }
    // ==============================================================================

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

    // ==================== MODIFIED: CONFIGURE ENEMY METHOD ====================
    private void ConfigureEnemy(GameObject enemy)
    {
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent == null) return;

        // Configure Melee Enemies
        if (enemy.GetComponent<MeleeEnemy>() != null)
        {
            enemyComponent.weapon = meleeWeapon;
            var meleeEnemy = enemy.GetComponent<MeleeEnemy>();

            // ENHANCED: Use new configuration method
            if (currentLevel >= 3)
            {
                meleeEnemy.ConfigureMeleeEnemy(2f, 0.25f, 3f, 2.5f); // Enhanced movement
            }
            else
            {
                meleeEnemy.ConfigureMeleeEnemy(2f, 0.25f, 1f, 1.5f); // Basic movement
            }

            enemyComponent.SetEnemyType(EnemyType.Melee);
        }
        // MODIFIED: Configure Machine Gun Enemies with level-based difficulty
        else if (enemy.GetComponent<MachineGunEnemy>() != null)
        {
            var machineGun = enemy.GetComponent<MachineGunEnemy>();

            if (currentLevel == 4) // NERFED VERSION for level 4 introduction
            {
                // Reduced power for first encounter
                machineGun.ConfigureShooter(1f, 6f, 2f, 10f); // damage, range, rate, speed
                machineGun.ConfigureMachineGun(2f, 25f, 2f); // rate, inaccuracy, burst duration
                Debug.Log("Spawned NERFED Machine Gun for Level 4");
            }
            else if (currentLevel >= 5) // FULL POWER for level 5+
            {
                // Original challenging settings
                machineGun.ConfigureShooter(1f, 8f, 5f, 10f);
                machineGun.ConfigureMachineGun(5f, 15f, 3f);
                Debug.Log("Spawned FULL POWER Machine Gun for Level 5+");
            }

            enemyComponent.SetEnemyType(EnemyType.MachineGun);
        }
        // Configure Sniper Enemies
        else if (enemy.GetComponent<SniperEnemy>() != null)
        {
            var sniper = enemy.GetComponent<SniperEnemy>();

            // ENHANCED: Use advanced configuration
            if (currentLevel >= 5)
            {
                sniper.ConfigureSniperAdvanced(1.5f, 1f, 3f, 2); // Repositions after 2 shots
            }
            else
            {
                sniper.ConfigureSniperAdvanced(1.8f, 2f, 2f, 4); // Slower, less accurate, repositions less
            }

            enemyComponent.SetEnemyType(EnemyType.Shooter);
        }
        // Configure Exploder Enemies
        else if (enemy.GetComponent<ExploderEnemy>() != null)
        {
            var exploder = enemy.GetComponent<ExploderEnemy>();

            // ENHANCED: Level-based configuration
            if (currentLevel == 2)
            {
                exploder.ConfigureExploder(2f, 1.5f, 1.2f); // Reduced damage and longer fuse for introduction
            }
            else
            {
                exploder.ConfigureExploder(2.5f, 2f, 1f); // Full power
            }

            enemyComponent.SetEnemyType(EnemyType.Exploder);
        }
    }
    // ==========================================================================
}