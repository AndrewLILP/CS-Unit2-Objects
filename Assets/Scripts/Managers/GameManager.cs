using UnityEngine;

/// <summary>
/// GameManager is responsible for managing the game state and flow.
/// </summary>

public class GameManager : MonoBehaviour
{
    [Header("Game Entities")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Game Variables")]
    [SerializeField] private float enemySpawnRate;

    public ScoreManager scoreManager;

    private GameObject tempEnemy;
    private bool isEnemySpawning;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isEnemySpawning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))    
        {
            CreateEnemy();
        }
    }

    void CreateEnemy()
    {
        tempEnemy = Instantiate(enemyPrefab);
        tempEnemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        tempEnemy.GetComponent<Enemy>().weapon = meleeWeapon;
        tempEnemy.GetComponent<MeleeEnemy>().SetMeleeEnemy(2, 0.25f);
    }
}
