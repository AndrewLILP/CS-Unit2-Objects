using UnityEngine;

// ==================== CREATED FOR Story 2.2: Game Over & High Score ====================
// This script helps test all the Game Over functionality implemented in Story 2.2
// ========================================================================================

public class GameOverTester : MonoBehaviour
{
    [Header("Story 2.2 Test Controls")]
    [SerializeField] private bool enableTesting = true;
    [SerializeField] private KeyCode triggerGameOverKey = KeyCode.G;
    [SerializeField] private KeyCode addScoreKey = KeyCode.S;
    [SerializeField] private KeyCode killPlayerKey = KeyCode.K;
    [SerializeField] private KeyCode damagePlayerKey = KeyCode.D;
    [SerializeField] private KeyCode healPlayerKey = KeyCode.H;

    [Header("Story 2.2 Test Values")]
    [SerializeField] private int scoreToAdd = 10;
    [SerializeField] private float damageAmount = 25f;
    [SerializeField] private float healAmount = 30f;

    private GameManager gameManager;
    private ScoreManager scoreManager;
    private Player player;
    private UIManager uiManager;

    private void Start()
    {
        if (!enableTesting) return;

        InitializeReferences();
        LogTestingControls();
    }

    private void InitializeReferences()
    {
        gameManager = GameManager.GetInstance();
        if (gameManager != null)
        {
            scoreManager = gameManager.scoreManager;
            player = gameManager.GetPlayer();
            uiManager = gameManager.uiManager;
        }

        ValidateStory22Setup();
    }

    private void LogTestingControls()
    {
        Debug.Log("🧪 STORY 2.2 GAME OVER TESTER ACTIVE");
        Debug.Log($"Controls: {triggerGameOverKey}=Game Over, {addScoreKey}=Add Score, {killPlayerKey}=Kill Player");
        Debug.Log($"Additional: {damagePlayerKey}=Damage, {healPlayerKey}=Heal");
    }

    private void ValidateStory22Setup()
    {
        Debug.Log("=== STORY 2.2 VALIDATION ===");
        Debug.Log($"GameManager: {(gameManager != null ? "✅" : "❌")}");
        Debug.Log($"ScoreManager: {(scoreManager != null ? "✅" : "❌")}");
        Debug.Log($"Player: {(player != null ? "✅" : "❌")}");
        Debug.Log($"UIManager: {(uiManager != null ? "✅" : "❌")}");

        if (uiManager != null)
        {
            // Check if UIManager has Story 2.2 methods
            var hasGameOverMethod = uiManager.GetType().GetMethod("IsGameOver") != null;
            var hasForceGameOverMethod = uiManager.GetType().GetMethod("ForceGameOver") != null;

            Debug.Log($"UIManager Story 2.2 Extensions: {(hasGameOverMethod && hasForceGameOverMethod ? "✅" : "❌")}");

            if (!hasGameOverMethod)
            {
                Debug.LogError("❌ UIManager missing Story 2.2 extensions! Add the new methods.");
            }
        }
        else
        {
            Debug.LogError("❌ UIManager missing! Check GameManager.uiManager assignment");
        }
    }

    private void Update()
    {
        if (!enableTesting) return;

        HandleTestingInput();
    }

    private void HandleTestingInput()
    {
        if (Input.GetKeyDown(triggerGameOverKey))
        {
            TestGameOver();
        }

        if (Input.GetKeyDown(addScoreKey))
        {
            TestAddScore();
        }

        if (Input.GetKeyDown(killPlayerKey))
        {
            TestKillPlayer();
        }

        if (Input.GetKeyDown(damagePlayerKey))
        {
            TestDamagePlayer();
        }

        if (Input.GetKeyDown(healPlayerKey))
        {
            TestHealPlayer();
        }
    }

    // ==================== STORY 2.2 TEST METHODS ====================

    [ContextMenu("Test Game Over")]
    public void TestGameOver()
    {
        Debug.Log("🧪 STORY 2.2 TEST: Force Game Over");

        if (uiManager != null && uiManager.GetType().GetMethod("ForceGameOver") != null)
        {
            uiManager.ForceGameOver();
        }
        else if (gameManager != null)
        {
            gameManager.StopGame();
        }
        else
        {
            Debug.LogError("Cannot test game over - missing references");
        }
    }

    [ContextMenu("Test Add Score")]
    public void TestAddScore()
    {
        Debug.Log($"🧪 STORY 2.2 TEST: Add {scoreToAdd} points");

        if (scoreManager != null)
        {
            for (int i = 0; i < scoreToAdd; i++)
            {
                scoreManager.IncrementScore();
            }
            Debug.Log($"Score is now: {scoreManager.GetScore()}");
        }
        else
        {
            Debug.LogError("Cannot test score - ScoreManager missing");
        }
    }

    [ContextMenu("Test Kill Player")]
    public void TestKillPlayer()
    {
        Debug.Log("🧪 STORY 2.2 TEST: Kill Player (should trigger Game Over)");

        if (player != null)
        {
            float healthBefore = player.health.GetHealth();
            player.GetDamage(999f); // Massive damage to ensure death

            Debug.Log($"Player health: {healthBefore} → {player.health.GetHealth()}");
            Debug.Log("Player should die and trigger Story 2.2 Game Over screen");
        }
        else
        {
            Debug.LogError("Cannot test kill player - Player missing");
        }
    }

    [ContextMenu("Test Damage Player")]
    public void TestDamagePlayer()
    {
        Debug.Log($"🧪 STORY 2.2 TEST: Damage player for {damageAmount}");

        if (player != null)
        {
            float healthBefore = player.health.GetHealth();
            player.GetDamage(damageAmount);
            float healthAfter = player.health.GetHealth();

            Debug.Log($"Health: {healthBefore} → {healthAfter}");

            if (healthAfter <= 0)
            {
                Debug.Log("Player health at 0 - should trigger Game Over");
            }
        }
        else
        {
            Debug.LogError("Cannot test damage - Player missing");
        }
    }

    [ContextMenu("Test Heal Player")]
    public void TestHealPlayer()
    {
        Debug.Log($"🧪 STORY 2.2 TEST: Heal player for {healAmount}");

        if (player != null)
        {
            float healthBefore = player.health.GetHealth();
            player.health.AddHealth(healAmount);
            float healthAfter = player.health.GetHealth();

            Debug.Log($"Health: {healthBefore} → {healthAfter}");

            if (uiManager != null)
            {
                uiManager.UpdateHealth(healthAfter);
            }
        }
        else
        {
            Debug.LogError("Cannot test healing - Player missing");
        }
    }

    [ContextMenu("Test High Score Scenario")]
    public void TestHighScoreScenario()
    {
        Debug.Log("🧪 STORY 2.2 TEST: Set up NEW HIGH SCORE scenario");

        if (scoreManager != null)
        {
            int currentHighScore = scoreManager.GetHighScore();
            int targetScore = currentHighScore + 50;

            Debug.Log($"Current High Score: {currentHighScore}");
            Debug.Log($"Setting score to: {targetScore}");

            // Add enough points to beat high score
            for (int i = scoreManager.GetScore(); i < targetScore; i++)
            {
                scoreManager.IncrementScore();
            }

            Debug.Log($"New score: {scoreManager.GetScore()}");
            Debug.Log("🎯 Now trigger Game Over to see 'NEW HIGH SCORE!' message");
        }
        else
        {
            Debug.LogError("Cannot test high score - ScoreManager missing");
        }
    }

    [ContextMenu("Reset High Score")]
    public void ResetHighScore()
    {
        Debug.Log("🧪 STORY 2.2 TEST: Reset high score to 0");
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
        Debug.Log("High score reset - any score will now be a new high score");
    }

    [ContextMenu("Validate Story 2.2 Setup")]
    public void ValidateStory22SetupMenu()
    {
        ValidateStory22Setup();
    }

    [ContextMenu("Run Full Story 2.2 Test")]
    public void RunFullStory22Test()
    {
        StartCoroutine(FullTestSequence());
    }

    private System.Collections.IEnumerator FullTestSequence()
    {
        Debug.Log("🚀 RUNNING FULL STORY 2.2 TEST SEQUENCE");

        // Step 1: Validate setup
        ValidateStory22Setup();
        yield return new WaitForSeconds(1f);

        // Step 2: Test score addition
        TestAddScore();
        yield return new WaitForSeconds(1f);

        // Step 3: Test health system
        TestDamagePlayer();
        yield return new WaitForSeconds(1f);
        TestHealPlayer();
        yield return new WaitForSeconds(1f);

        // Step 4: Set up high score scenario
        TestHighScoreScenario();
        yield return new WaitForSeconds(2f);

        // Step 5: Trigger game over
        Debug.Log("🎯 Testing Game Over with new high score in 3 seconds...");
        yield return new WaitForSeconds(3f);

        TestGameOver();

        Debug.Log("✅ FULL STORY 2.2 TEST SEQUENCE COMPLETE");
        Debug.Log("👀 Check that Game Over screen shows with 'NEW HIGH SCORE!' message");
    }

    // ==================== STORY 2.2 UI DISPLAY ====================
    private void OnGUI()
    {
        if (!enableTesting) return;

        GUI.color = Color.white;
        GUILayout.BeginArea(new Rect(10, 10, 350, 250));

        GUILayout.Label("🧪 STORY 2.2: GAME OVER TESTER", GUI.skin.box);
        GUILayout.Space(5);

        GUILayout.Label($"🎮 Press {triggerGameOverKey} - Force Game Over");
        GUILayout.Label($"📊 Press {addScoreKey} - Add {scoreToAdd} Score");
        GUILayout.Label($"💀 Press {killPlayerKey} - Kill Player");
        GUILayout.Label($"💥 Press {damagePlayerKey} - Damage Player");
        GUILayout.Label($"❤️ Press {healPlayerKey} - Heal Player");

        GUILayout.Space(10);

        if (scoreManager != null)
        {
            GUILayout.Label($"📈 Current Score: {scoreManager.GetScore()}");
            GUILayout.Label($"🏆 High Score: {scoreManager.GetHighScore()}");
        }

        if (player != null)
        {
            GUILayout.Label($"❤️ Player Health: {player.health.GetHealth():F1}");
        }

        if (uiManager != null)
        {
            var hasStory22Methods = uiManager.GetType().GetMethod("IsGameOver") != null;
            GUILayout.Label($"🎯 Story 2.2 Ready: {(hasStory22Methods ? "✅ YES" : "❌ NO")}");
        }

        GUILayout.EndArea();
    }
}