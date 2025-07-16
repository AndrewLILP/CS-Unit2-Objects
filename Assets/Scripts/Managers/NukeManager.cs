// NukeManager.cs - SIMPLE VERSION
// Replace your entire NukeManager.cs with this simple version

using UnityEngine;
using System.Collections;
using System;

public class NukeManager : MonoBehaviour
{
    [Header("Nuke Settings")]
    [SerializeField] private int maxNukes = 5;
    [SerializeField] private Transform nukeSpawnPoint; // Single spawn point set in inspector
    [SerializeField] private GameObject nukePickupPrefab; // Nuke pickup prefab

    [Header("Audio (Optional)")]
    [SerializeField] private AudioClip nukeExplosionSound;
    [SerializeField] private AudioSource audioSource;

    private int currentNukes = 0;

    // Events for UI updates
    public Action<int> OnNukeCountChanged;

    // Singleton pattern
    private static NukeManager instance;
    public static NukeManager GetInstance() => instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Setup audio source if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    private void Start()
    {
        // Subscribe to game events
        GameManager.GetInstance().OnGameStart += ResetNukes;

        // Spawn initial nuke pickup at milestones
        ScoreManager scoreManager = GameManager.GetInstance().scoreManager;
        if (scoreManager != null)
        {
            scoreManager.OnScoreUpdate.AddListener(CheckForNukeSpawn);
        }
    }

    // ========== CORE NUKE METHODS ==========

    public void AddNuke()
    {
        if (currentNukes < maxNukes)
        {
            currentNukes++;
            OnNukeCountChanged?.Invoke(currentNukes);
            Debug.Log($"Nuke collected! Total: {currentNukes}");
        }
        else
        {
            Debug.Log("Maximum nukes reached!");
        }
    }

    public bool CanUseNuke()
    {
        return currentNukes > 0;
    }

    public void UseNuke()
    {
        if (!CanUseNuke())
        {
            Debug.Log("No nukes available!");
            return;
        }

        currentNukes--;
        OnNukeCountChanged?.Invoke(currentNukes);

        StartCoroutine(ExecuteNuke());
    }

    public int GetNukeCount() => currentNukes;

    // ========== NUKE EXECUTION ==========

    private IEnumerator ExecuteNuke()
    {
        Debug.Log("NUKE ACTIVATED!");

        // Play explosion sound
        if (nukeExplosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(nukeExplosionSound);
        }

        // Screen shake
        CameraShake.ShakeExplosion();

        yield return new WaitForSeconds(0.2f);

        // Destroy all enemies
        Enemy[] enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy enemy in enemies)
        {
            if (enemy != null)
            {
                EffectsManager.PlayEnemyDeath(enemy.transform.position);
                GameManager.GetInstance().scoreManager.IncrementScore();
                Destroy(enemy.gameObject);
            }
        }

        // Destroy all pickups
        Pickup[] pickups = FindObjectsByType<Pickup>(FindObjectsSortMode.None);
        foreach (Pickup pickup in pickups)
        {
            if (pickup != null)
            {
                Destroy(pickup.gameObject);
            }
        }

        Debug.Log($"NUKE COMPLETE! Destroyed {enemies.Length} enemies and {pickups.Length} pickups!");
    }

    // ========== SIMPLE MILESTONE SPAWNING ==========

    private void CheckForNukeSpawn()
    {
        int currentScore = GameManager.GetInstance().scoreManager.GetScore();

        // Automatically give player a nuke when score reaches 3
        if (currentScore == 3)
        {
            AddNuke(); // Add directly to inventory
            Debug.Log("🎯 SCORE 3 REACHED! Nuke automatically added to inventory!");
        }
    }

    private void SpawnNukePickup()
    {
        if (nukePickupPrefab == null)
        {
            Debug.LogError("No nuke pickup prefab assigned!");
            return;
        }

        if (nukeSpawnPoint == null)
        {
            Debug.LogError("No nuke spawn point assigned!");
            return;
        }

        // Spawn nuke at the designated spawn point
        GameObject spawnedNuke = Instantiate(nukePickupPrefab, nukeSpawnPoint.position, Quaternion.identity);

        // ========== NEW: Add obvious spawn effects ==========
        // Screen shake to get attention
        CameraShake.ShakeHeavy();

        // Visual effect at spawn location  
        EffectsManager.PlayHitEffect(nukeSpawnPoint.position, false);

        // Multiple effects for visibility
        for (int i = 0; i < 3; i++)
        {
            Vector3 offset = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f),
                0f
            );
            EffectsManager.PlayExplosion(nukeSpawnPoint.position + offset);
        }

        Debug.Log($"🎯 MILESTONE NUKE SPAWNED with BIG effects at {nukeSpawnPoint.position}!");
    }
    // ========== RESET AND CLEANUP ==========

    private void ResetNukes()
    {
        currentNukes = 0;
        OnNukeCountChanged?.Invoke(currentNukes);
    }

    private void OnDestroy()
    {
        if (GameManager.GetInstance() != null)
        {
            GameManager.GetInstance().OnGameStart -= ResetNukes;

            ScoreManager scoreManager = GameManager.GetInstance().scoreManager;
            if (scoreManager != null)
            {
                scoreManager.OnScoreUpdate.RemoveListener(CheckForNukeSpawn);
            }
        }
    }
}