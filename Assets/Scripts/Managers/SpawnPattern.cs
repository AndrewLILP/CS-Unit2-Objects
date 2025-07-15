using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SpawnPattern
{
    public string patternName;
    public SpawnGroup[] spawnGroups;
    public float delayBetweenGroups = 0.5f;
    public int minLevel = 1; // Minimum level to use this pattern
    public int maxLevel = 99; // Maximum level to use this pattern
    public float patternWeight = 1f; // Probability weight for selection
}

[System.Serializable]
public class SpawnGroup
{
    public EnemyType[] enemyTypes;
    public int[] spawnPointIndices; // Which spawn points to use
    public float groupDelay = 0f; // Delay before spawning this group
    public bool simultaneousSpawn = true; // Spawn all at once or with small delays
}

public class SpawnPatternSystem : MonoBehaviour
{
    [Header("Spawn Patterns")]
    [SerializeField] private SpawnPattern[] spawnPatterns;

    [Header("Pattern Frequency")]
    [SerializeField] private float patternChance = 0.3f; // Chance to use pattern vs single spawn
    [SerializeField] private float patternCooldown = 5f; // Min time between patterns

    private GameManager gameManager;
    private float lastPatternTime = 0f;

    private void Start()
    {
        gameManager = GameManager.GetInstance();
        InitializeDefaultPatterns();
    }

    private void InitializeDefaultPatterns()
    {
        // Create default patterns if none are assigned
        if (spawnPatterns == null || spawnPatterns.Length == 0)
        {
            CreateDefaultPatterns();
        }
    }

    private void CreateDefaultPatterns()
    {
        List<SpawnPattern> patterns = new List<SpawnPattern>();

        // Pattern 1: "The Pincer" (Level 3+)
        patterns.Add(new SpawnPattern
        {
            patternName = "The Pincer",
            minLevel = 3,
            maxLevel = 99,
            patternWeight = 1f,
            delayBetweenGroups = 0.2f,
            spawnGroups = new SpawnGroup[]
            {
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.Melee },
                    spawnPointIndices = new int[] { 0 }, // First spawn point
                    groupDelay = 0f,
                    simultaneousSpawn = true
                },
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.Melee },
                    spawnPointIndices = new int[] { 2 }, // Opposite spawn point
                    groupDelay = 0.2f,
                    simultaneousSpawn = true
                }
            }
        });

        // Pattern 2: "Cover Fire" (Level 4+)
        patterns.Add(new SpawnPattern
        {
            patternName = "Cover Fire",
            minLevel = 4,
            maxLevel = 99,
            patternWeight = 1f,
            delayBetweenGroups = 0.5f,
            spawnGroups = new SpawnGroup[]
            {
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.Shooter },
                    spawnPointIndices = new int[] { 1 }, // Back spawn point
                    groupDelay = 0f,
                    simultaneousSpawn = true
                },
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.Melee, EnemyType.Melee },
                    spawnPointIndices = new int[] { 0, 2 }, // Side spawn points
                    groupDelay = 0.8f,
                    simultaneousSpawn = false
                }
            }
        });

        // Pattern 3: "The Ambush" (Level 5+)
        patterns.Add(new SpawnPattern
        {
            patternName = "The Ambush",
            minLevel = 5,
            maxLevel = 99,
            patternWeight = 1f,
            delayBetweenGroups = 0.3f,
            spawnGroups = new SpawnGroup[]
            {
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.Shooter },
                    spawnPointIndices = new int[] { 3 }, // Far spawn point
                    groupDelay = 0f,
                    simultaneousSpawn = true
                },
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.Exploder },
                    spawnPointIndices = new int[] { 1 }, // Medium spawn point
                    groupDelay = 0.5f,
                    simultaneousSpawn = true
                },
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.Melee },
                    spawnPointIndices = new int[] { 0 }, // Close spawn point
                    groupDelay = 1f,
                    simultaneousSpawn = true
                }
            }
        });

        // Pattern 4: "Overwhelming Force" (Level 6+)
        patterns.Add(new SpawnPattern
        {
            patternName = "Overwhelming Force",
            minLevel = 6,
            maxLevel = 99,
            patternWeight = 0.7f, // Less frequent due to intensity
            delayBetweenGroups = 0.4f,
            spawnGroups = new SpawnGroup[]
            {
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.MachineGun },
                    spawnPointIndices = new int[] { 2 },
                    groupDelay = 0f,
                    simultaneousSpawn = true
                },
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.Melee, EnemyType.Exploder },
                    spawnPointIndices = new int[] { 0, 1 },
                    groupDelay = 0.6f,
                    simultaneousSpawn = false
                },
                new SpawnGroup
                {
                    enemyTypes = new EnemyType[] { EnemyType.Shooter },
                    spawnPointIndices = new int[] { 3 },
                    groupDelay = 1.2f,
                    simultaneousSpawn = true
                }
            }
        });

        spawnPatterns = patterns.ToArray();
    }

    public bool ShouldUsePattern(int currentLevel)
    {
        // Check cooldown
        if (Time.time - lastPatternTime < patternCooldown)
            return false;

        // Check if we should use a pattern based on chance
        if (Random.Range(0f, 1f) > patternChance)
            return false;

        // Check if any patterns are available for current level
        return GetAvailablePatterns(currentLevel).Count > 0;
    }

    public void ExecuteRandomPattern(int currentLevel)
    {
        List<SpawnPattern> availablePatterns = GetAvailablePatterns(currentLevel);

        if (availablePatterns.Count == 0)
        {
            Debug.LogWarning("No spawn patterns available for level " + currentLevel);
            return;
        }

        // Select pattern based on weights
        SpawnPattern selectedPattern = SelectWeightedPattern(availablePatterns);

        if (selectedPattern != null)
        {
            StartCoroutine(ExecutePattern(selectedPattern));
            lastPatternTime = Time.time;
            Debug.Log($"Executing spawn pattern: {selectedPattern.patternName}");
        }
    }

    private List<SpawnPattern> GetAvailablePatterns(int currentLevel)
    {
        List<SpawnPattern> available = new List<SpawnPattern>();

        foreach (SpawnPattern pattern in spawnPatterns)
        {
            if (currentLevel >= pattern.minLevel && currentLevel <= pattern.maxLevel)
            {
                available.Add(pattern);
            }
        }

        return available;
    }

    private SpawnPattern SelectWeightedPattern(List<SpawnPattern> patterns)
    {
        float totalWeight = 0f;
        foreach (SpawnPattern pattern in patterns)
        {
            totalWeight += pattern.patternWeight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (SpawnPattern pattern in patterns)
        {
            currentWeight += pattern.patternWeight;
            if (randomValue <= currentWeight)
            {
                return pattern;
            }
        }

        return patterns[0]; // Fallback
    }

    private IEnumerator ExecutePattern(SpawnPattern pattern)
    {
        foreach (SpawnGroup group in pattern.spawnGroups)
        {
            yield return new WaitForSeconds(group.groupDelay);

            if (group.simultaneousSpawn)
            {
                // Spawn all enemies in group at once
                for (int i = 0; i < group.enemyTypes.Length; i++)
                {
                    SpawnEnemyAtPoint(group.enemyTypes[i], group.spawnPointIndices[i % group.spawnPointIndices.Length]);
                }
            }
            else
            {
                // Spawn enemies with small delays
                for (int i = 0; i < group.enemyTypes.Length; i++)
                {
                    SpawnEnemyAtPoint(group.enemyTypes[i], group.spawnPointIndices[i % group.spawnPointIndices.Length]);
                    if (i < group.enemyTypes.Length - 1) // Don't wait after last enemy
                    {
                        yield return new WaitForSeconds(0.2f);
                    }
                }
            }

            yield return new WaitForSeconds(pattern.delayBetweenGroups);
        }
    }

    private void SpawnEnemyAtPoint(EnemyType enemyType, int spawnPointIndex)
    {
        if (gameManager != null)
        {
            gameManager.SpawnSpecificEnemyAtPoint(enemyType, spawnPointIndex);
        }
    }
}