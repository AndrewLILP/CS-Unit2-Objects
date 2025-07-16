using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
// ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// ======================================================================================

public class UIManager : MonoBehaviour
{
    // ==================== EXISTING FIELDS (Pre-Story 2.2) ====================
    [Header("In-Game UI")]
    [SerializeField] private TMP_Text txtHealth;
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private TMP_Text txtHighScore;
    [SerializeField] private TMP_Text txtNukeCount;
    [SerializeField] Player player;
    [SerializeField] private GameObject menuCanvas;
    // =========================================================================

    // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
    [Header("Game Over UI - Story 2.2")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalHighScoreText;
    [SerializeField] private TMP_Text newHighScoreLabel;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Game Over Settings - Story 2.2")]
    [SerializeField] private float panelFadeInDuration = 1f;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private AnimationCurve fadeInCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Audio - Story 2.2")]
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioSource audioSource;
    // ======================================================================================

    // ==================== EXISTING VARIABLES (Pre-Story 2.2) ====================
    private ScoreManager scoreManager;
    // =============================================================================

    // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
    private bool isGameOver = false;
    // ======================================================================================

    private void Start()
    {
        // ==================== EXISTING INITIALIZATION (Pre-Story 2.2) ====================
        UpdateHealth(player.health.GetHealth());
        scoreManager = GameManager.GetInstance().scoreManager;

        GameManager.GetInstance().OnGameStart += GameStarted;
        GameManager.GetInstance().OnGameOver += GameOver;

        if (scoreManager != null)
        {
            scoreManager.OnScoreUpdate.AddListener(UpdateScore);
        }

        if (NukeManager.GetInstance() != null)
        {
            NukeManager.GetInstance().OnNukeCountChanged += UpdateNukeCount;
        }

        UpdateNukeCount(0);
        // ==================================================================================

        // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
        InitializeGameOverUI();
        SetupAudioSource();
        // ======================================================================================
    }

    // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
    private void InitializeGameOverUI()
    {
        // Hide game over panel initially
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Hide new high score label initially
        if (newHighScoreLabel != null)
        {
            newHighScoreLabel.gameObject.SetActive(false);
        }

        // Setup button listeners
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        }

        Debug.Log("Game Over UI initialized successfully");
    }

    private void SetupAudioSource()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    // ======================================================================================

    // ==================== EXISTING METHOD - ENHANCED FOR Story 2.2 ====================
    public void GameStarted()
    {
        Debug.Log("GameStarted called!");

        if (player == null)
        {
            Debug.LogError("Player is null in GameStarted!");
            return;
        }

        Debug.Log("Player found, current health: " + player.health.GetHealth());
        UpdateHealth(player.health.GetHealth());
        player.OnHealthUpdate += UpdateHealth;
        Debug.Log("Subscribed to health updates");

        // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
        // Reset game over state when game starts
        isGameOver = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (newHighScoreLabel != null)
        {
            newHighScoreLabel.gameObject.SetActive(false);
        }
        // ======================================================================================
    }

    // ==================== EXISTING METHOD - ENHANCED FOR Story 2.2 ====================
    public void GameOver()
    {
        // ==================== EXISTING LOGIC (Pre-Story 2.2) ====================
        if (player != null)
        {
            player.OnHealthUpdate -= UpdateHealth;
        }
        // =====================================================================

        // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
        if (isGameOver) return; // Prevent multiple calls
        isGameOver = true;

        Debug.Log("UIManager: Enhanced Game Over triggered");
        StartCoroutine(ShowGameOverScreen());
        // ======================================================================================
    }

    // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
    private IEnumerator ShowGameOverScreen()
    {
        // Wait for death effects to complete
        yield return new WaitForSeconds(1f);

        // Play game over sound
        PlaySound(gameOverSound);

        // Show and animate the game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            yield return StartCoroutine(AnimateGameOverPanel());
        }

        // Update final scores
        UpdateFinalScores();

        // Check for new high score achievement
        CheckForNewHighScore();

        Debug.Log("Game Over screen fully displayed");
    }

    private IEnumerator AnimateGameOverPanel()
    {
        if (gameOverPanel == null) yield break;

        CanvasGroup canvasGroup = gameOverPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameOverPanel.AddComponent<CanvasGroup>();
        }

        // Fade in animation
        canvasGroup.alpha = 0f;
        float elapsed = 0f;

        while (elapsed < panelFadeInDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / panelFadeInDuration;
            canvasGroup.alpha = fadeInCurve.Evaluate(progress);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private void UpdateFinalScores()
    {
        int finalScore = scoreManager.GetScore();
        int highScore = scoreManager.GetHighScore();

        if (finalScoreText != null)
        {
            finalScoreText.text = $"Final Score: {finalScore}";
        }

        if (finalHighScoreText != null)
        {
            finalHighScoreText.text = $"High Score: {highScore}";
        }

        Debug.Log($"Final Score: {finalScore}, High Score: {highScore}");
    }

    private void CheckForNewHighScore()
    {
        int finalScore = scoreManager.GetScore();
        int previousHighScore = PlayerPrefs.GetInt("HighScore", 0);

        bool isNewHighScore = finalScore > previousHighScore;

        if (newHighScoreLabel != null)
        {
            newHighScoreLabel.gameObject.SetActive(isNewHighScore);

            if (isNewHighScore)
            {
                newHighScoreLabel.text = "NEW HIGH SCORE!";
                StartCoroutine(AnimateNewHighScoreLabel());
                Debug.Log("NEW HIGH SCORE ACHIEVED!");
            }
        }
    }

    private IEnumerator AnimateNewHighScoreLabel()
    {
        if (newHighScoreLabel == null) yield break;

        float duration = 3f;
        float elapsed = 0f;
        Vector3 originalScale = newHighScoreLabel.transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float scale = 1f + Mathf.Sin(elapsed * 8f) * 0.15f; // Pulsing effect
            newHighScoreLabel.transform.localScale = originalScale * scale;
            yield return null;
        }

        newHighScoreLabel.transform.localScale = originalScale;
    }

    // ==================== BUTTON HANDLERS FOR Story 2.2 ====================
    public void OnRestartClicked()
    {
        Debug.Log("Restart button clicked");
        PlaySound(buttonClickSound);
        StartCoroutine(RestartGame());
    }

    public void OnMainMenuClicked()
    {
        Debug.Log("Main Menu button clicked");
        PlaySound(buttonClickSound);
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(0.1f); // Button feedback delay

        // Reset game over state
        isGameOver = false;

        // Hide game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        // Restart through GameManager
        if (GameManager.GetInstance() != null)
        {
            GameManager.GetInstance().RestartGame();
        }

        Debug.Log("Game restarted via UI");
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(0.1f); // Button feedback delay

        try
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load main menu: {e.Message}");
            // Fallback: restart current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // ==================== UTILITY METHODS FOR Story 2.2 ====================
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void ForceGameOver()
    {
        if (!isGameOver)
        {
            GameOver();
        }
    }
    // ======================================================================================

    // ==================== EXISTING METHODS (Pre-Story 2.2) ====================
    public void UpdateHighScore()
    {
        txtHighScore.SetText(scoreManager.GetHighScore().ToString());
    }

    public void UpdateHealth(float currentHealth)
    {
        Debug.Log("UpdateHealth called with: " + currentHealth);
        txtHealth.SetText(currentHealth.ToString());
    }

    public void UpdateScore()
    {
        txtScore.SetText(GameManager.GetInstance().scoreManager.GetScore().ToString());
    }

    private void UpdateNukeCount(int nukeCount)
    {
        if (txtNukeCount != null)
        {
            txtNukeCount.SetText($"Nukes: {nukeCount}");
        }
    }
    // =======================================================================

    // ==================== ENHANCED CLEANUP FOR Story 2.2 ====================
    private void OnDisable()
    {
        // ==================== EXISTING CLEANUP (Pre-Story 2.2) ====================
        if (player != null)
            player.OnHealthUpdate -= UpdateHealth;

        if (scoreManager != null)
        {
            scoreManager.OnScoreUpdate.RemoveListener(UpdateScore);
        }

        if (NukeManager.GetInstance() != null)
        {
            NukeManager.GetInstance().OnNukeCountChanged -= UpdateNukeCount;
        }

        if (GameManager.GetInstance() != null)
        {
            GameManager.GetInstance().OnGameStart -= GameStarted;
            GameManager.GetInstance().OnGameOver -= GameOver;
        }
        // ==============================================================================

        // ==================== ADDED FOR Story 2.2: Game Over & High Score ====================
        // Clean up button listeners
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
        }
        // ======================================================================================
    }
}