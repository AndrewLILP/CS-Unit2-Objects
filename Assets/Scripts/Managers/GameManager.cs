using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using System;

// ==================== ENHANCED FOR Story 2.2: Game Over & High Score ====================
using UnityEngine.SceneManagement; // Added for restart functionality
// ======================================================================================

/// <summary>
/// GameManager is responsible for managing the game state and flow.
/// Enhanced for Story 2.2 with restart and cleanup functionality.
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Game Entities")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Game Variables")]
    [SerializeField] private float enemySpawnRate;

    [Header("Difficulty System")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int pointsPerLevel = 20;

    [Header("Spawn Pattern System")]
    [SerializeField] private SpawnPatternSystem spawnPatternSystem;

    public ScoreManager scoreManager;
    public PickupManager pickupManager;
    public UIManager uiManager;

    public Action OnGameStart;
    public Action OnGameOver;

    private GameObject tempEnemy;
    private bool isEnemySpawning;
    private bool isPlaying;

    // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
    private bool isGameOver = false;
    // ======================================================================================

    private Weapon meleeWeapon = new Weapon("Melee", 1, 0);
    private Weapon exploderWeapon = new Weapon("Exploder", 2, 8);
    private Weapon machineGunWeapon = new Weapon("Machine Gun", 1, 10);

    // Singleton
    private static GameManager instance;
    public static GameManager GetInstance()
    {
        return instance;
    }

    void SetSingleton()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
    }

    private void Awake()
    {
        SetSingleton();

        if (spawnPatternSystem == null)
        {
            spawnPatternSystem = GetComponent<SpawnPatternSystem>();
            if (spawnPatternSystem == null)
            {
                spawnPatternSystem = gameObject.AddComponent<SpawnPatternSystem>();
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
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

    // ==================== ENHANCED FOR Story 2.2: Game Over & High Score ====================
    public void StartGame()
    {
        // Reset game over state
        isGameOver = false;

        player.gameObject.SetActive(true);
        player.OnDeath += StopGame;
        isPlaying = true;

        currentLevel = 1;
        enemySpawnRate = GetSpawnRateForLevel();

        OnGameStart?.Invoke();
        StartCoroutine(GameStarter());
    }
    // ======================================================================================

    IEnumerator GameStarter()
    {
        yield return new WaitForSeconds(2.0f);
        if (player.health.GetHealth() <= 0)
        {
            player.health.AddHealth(100);
        }
        isEnemySpawning = true;
        StartCoroutine(EnemySpawner());
    }
    // ==================== ENHANCED StopGame for Story 2.2 ====================
    // Replace your existing StopGame method with this enhanced version

    public void StopGame()
    {
        Debug.Log("GameManager: StopGame called - triggering Story 2.2 Game Over");

        // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
        // IMMEDIATELY stop enemy spawning and set game over flag
        isEnemySpawning = false;
        isGameOver = true;

        // Stop all coroutines to prevent new enemy spawns
        StopAllCoroutines();
        // ======================================================================================

        // Save high score
        scoreManager.SetHighScore();

        // Trigger UI game over BEFORE cleanup to show scores properly
        OnGameOver?.Invoke();

        // Start cleanup with delay for UI to show
        StartCoroutine(GameStopper());
    }

    // ==================== ENHANCED GameStopper for Story 2.2 ====================
    IEnumerator GameStopper()
    {
        // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
        // Ensure enemy spawning is stopped
        isEnemySpawning = false;

        // Give time for Game Over UI to display properly
        yield return new WaitForSeconds(3.0f);
        // ======================================================================================

        isPlaying = false;

        // Clean up existing enemies (they might still be trying to find player)
        CleanupGameObjects();
    }

    public void NotifyDeath(Enemy enemy)
    {
        pickupManager.SpawnPickup(enemy.transform.position);
    }

    // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
    public void RestartGame()
    {
        Debug.Log("GameManager: Restarting game for Story 2.2");

        // Stop any ongoing processes
        isEnemySpawning = false;
        StopAllCoroutines();

        // Clean up existing game objects
        CleanupGameObjects();

        // Reset player state
        ResetPlayer();

        // Reset game systems
        ResetGameSystems();

        // Start fresh game
        StartGame();

        Debug.Log("Game restart complete");
    }

    private void CleanupGameObjects()
    {
        // Clean up enemies
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.gameObject);
            }
        }

        // Clean up pickups
        Pickup[] pickups = FindObjectsByType<Pickup>(FindObjectsSortMode.None);
        foreach (Pickup pickup in pickups)
        {
            if (pickup != null)
            {
                Destroy(pickup.gameObject);
            }
        }

        Debug.Log($"Cleaned up {enemies.Length} enemies and {pickups.Length} pickups");
    }

    private void ResetPlayer()
    {
        if (player != null)
        {
            // Reactivate player
            player.gameObject.SetActive(true);

            // Reset health
            player.health = new Health(100f, 0.5f, 100f);

            // Reset player position (adjust Vector3.zero to your spawn point if needed)
            player.transform.position = Vector3.zero;

            // Clear any movement velocity
            Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
            if (playerRB != null)
            {
                playerRB.linearVelocity = Vector2.zero;
            }

            Debug.Log("Player reset for restart");
        }
    }

    private void ResetGameSystems()
    {
        // Reset score
        if (scoreManager != null)
        {
            scoreManager.ResetScore();
        }

        // Reset difficulty
        currentLevel = 1;
        enemySpawnRate = GetSpawnRateForLevel();

        // Reset game flags
        isPlaying = false;

        Debug.Log("Game systems reset for restart");
    }

    // Helper methods for Story 2.2
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public float GetCurrentSpawnRate()
    {
        return enemySpawnRate;
    }

    public bool IsGameActive()
    {
        return isPlaying && !isGameOver;
    }

    public string GetGameStats()
    {
        return $"Level: {currentLevel} | Spawn Rate: {enemySpawnRate:F1} | Playing: {isPlaying}";
    }
    // ======================================================================================

    // ==================== EXISTING METHODS (Pre-Story 2.2) ====================
    public void UpdateDifficulty()
    {
        int newLevel = (scoreManager.GetScore() / pointsPerLevel) + 1;
        if (newLevel > currentLevel)
        {
            currentLevel = newLevel;
            Debug.Log($"Level Up! Now on Level {currentLevel}");
            enemySpawnRate = GetSpawnRateForLevel();
        }
    }

    private float GetSpawnRateForLevel()
    {
        switch (currentLevel)
        {
            case 1: return 0.5f;
            case 2: return 0.7f;
            case 3: return 1.0f;
            case 4: return 1.2f;
            default:
                return Mathf.Min(2.0f, 1.2f + (currentLevel - 4) * 0.1f);
        }
    }

    private float[] GetEnemyWeights()
    {
        switch (currentLevel)
        {
            case 1:
                return new float[] { 1f, 0f, 0f, 0f };
            case 2:
                return new float[] { 0.6f, 0.4f, 0f, 0f };
            case 3:
                return new float[] { 0.4f, 0.3f, 0.3f, 0f };
            case 4:
                return new float[] { 0.3f, 0.2f, 0.3f, 0.2f };
            default:
                return new float[] { 0.25f, 0.25f, 0.25f, 0.25f };
        }
    }

    private GameObject GetEnemyForCurrentLevel()
    {
        float[] weights = GetEnemyWeights();
        float totalWeight = 0f;

        for (int i = 0; i < weights.Length && i < enemyPrefab.Length; i++)
            totalWeight += weights[i];

        if (totalWeight <= 0f) return enemyPrefab[0];

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        for (int i = 0; i < weights.Length && i < enemyPrefab.Length; i++)
        {
            currentWeight += weights[i];
            if (randomValue <= currentWeight)
                return enemyPrefab[i];
        }

        return enemyPrefab[0];
    }

    // ==================== ENHANCED CreateEnemy for Story 2.2 ====================
    void CreateEnemy()
    {
        // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
        // Safety check before creating enemies
        if (isGameOver || player == null || !player.gameObject.activeInHierarchy)
        {
            Debug.Log("Skipping enemy creation - game over or player inactive");
            return;
        }
        // ======================================================================================

        // Your existing CreateEnemy code...
        if (spawnPatternSystem != null && spawnPatternSystem.ShouldUsePattern(currentLevel))
        {
            spawnPatternSystem.ExecuteRandomPattern(currentLevel);
            return;
        }

        GameObject chosenEnemyPrefab = GetEnemyForCurrentLevel();
        tempEnemy = Instantiate(chosenEnemyPrefab);
        tempEnemy.transform.position = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].position;

        ConfigureEnemy(tempEnemy);
    }

    public void SpawnSpecificEnemyAtPoint(EnemyType enemyType, int spawnPointIndex)
    {
        GameObject enemyPrefab = GetEnemyPrefabByType(enemyType);
        if (enemyPrefab == null)
        {
            Debug.LogError($"No prefab found for enemy type: {enemyType}");
            return;
        }

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
    IEnumerator EnemySpawner()
    {
        while (isEnemySpawning)
        {
            yield return new WaitForSeconds(1.0f / enemySpawnRate);

            // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
            // Don't spawn enemies if game is over or player is dead
            if (isGameOver || player == null || !player.gameObject.activeInHierarchy)
            {
                Debug.Log("Stopping enemy spawn - game over or player inactive");
                break;
            }
            // ======================================================================================

            CreateEnemy();
        }
    }

    private void ConfigureEnemy(GameObject enemy)
    {
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent == null) return;

        if (enemy.GetComponent<MeleeEnemy>() != null)
        {
            enemyComponent.weapon = meleeWeapon;
            var meleeEnemy = enemy.GetComponent<MeleeEnemy>();

            if (currentLevel >= 3)
            {
                meleeEnemy.ConfigureMeleeEnemy(2f, 0.25f, 3f, 2.5f);
            }
            else
            {
                meleeEnemy.ConfigureMeleeEnemy(2f, 0.25f, 1f, 1.5f);
            }

            enemyComponent.SetEnemyType(EnemyType.Melee);
        }
        else if (enemy.GetComponent<MachineGunEnemy>() != null)
        {
            var machineGun = enemy.GetComponent<MachineGunEnemy>();

            if (currentLevel == 4)
            {
                machineGun.ConfigureShooter(1f, 6f, 2f, 10f);
                machineGun.ConfigureMachineGun(2f, 25f, 2f);
                Debug.Log("Spawned NERFED Machine Gun for Level 4");
            }
            else if (currentLevel >= 5)
            {
                machineGun.ConfigureShooter(1f, 8f, 5f, 10f);
                machineGun.ConfigureMachineGun(5f, 15f, 3f);
                Debug.Log("Spawned FULL POWER Machine Gun for Level 5+");
            }

            enemyComponent.SetEnemyType(EnemyType.MachineGun);
        }
        else if (enemy.GetComponent<SniperEnemy>() != null)
        {
            var sniper = enemy.GetComponent<SniperEnemy>();

            if (currentLevel >= 5)
            {
                sniper.ConfigureSniperAdvanced(1.5f, 1f, 3f, 2);
            }
            else
            {
                sniper.ConfigureSniperAdvanced(1.8f, 2f, 2f, 4);
            }

            enemyComponent.SetEnemyType(EnemyType.Shooter);
        }
        else if (enemy.GetComponent<ExploderEnemy>() != null)
        {
            var exploder = enemy.GetComponent<ExploderEnemy>();

            if (currentLevel == 2)
            {
                exploder.ConfigureExploder(2f, 1.5f, 1.2f);
            }
            else
            {
                exploder.ConfigureExploder(2.5f, 2f, 1f);
            }

            enemyComponent.SetEnemyType(EnemyType.Exploder);
        }
    }
    // =======================================================================
}