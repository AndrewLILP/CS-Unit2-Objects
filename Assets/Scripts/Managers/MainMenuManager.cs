using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button instructionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private Button backToMenuButton;

    [Header("Scene Management")]
    [SerializeField] private string gameSceneName = "SampleScene";
    [SerializeField] private bool useTransitionEffect = true;

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource buttonClickSound;

    private void Start()
    {
        Debug.Log("=== MAIN MENU MANAGER STARTING ===");
        StartCoroutine(InitializeMenuDelayed());
    }

    // Delayed initialization to ensure all components are ready
    private IEnumerator InitializeMenuDelayed()
    {
        yield return new WaitForEndOfFrame();

        DebugSystemComponents();
        InitializeMenu();
        SetupButtonListeners();

        Debug.Log("=== MAIN MENU INITIALIZATION COMPLETE ===");
    }

    private void DebugSystemComponents()
    {
        Debug.Log("=== SYSTEM COMPONENT CHECK ===");

        // Check essential Unity components
        var eventSystem = FindObjectOfType<UnityEngine.EventSystems.EventSystem>();
        var canvas = FindObjectOfType<Canvas>();
        var graphicRaycaster = canvas?.GetComponent<GraphicRaycaster>();

        Debug.Log($"EventSystem: {(eventSystem != null ? "✅ Found" : "❌ MISSING - CRITICAL ISSUE!")}");
        Debug.Log($"Canvas: {(canvas != null ? "✅ Found" : "❌ MISSING - CRITICAL ISSUE!")}");
        Debug.Log($"GraphicRaycaster: {(graphicRaycaster != null ? "✅ Found" : "❌ MISSING - CRITICAL ISSUE!")}");

        // Check button references
        Debug.Log("=== BUTTON REFERENCE CHECK ===");
        Debug.Log($"Start Button: {GetButtonStatus(startButton)}");
        Debug.Log($"Instructions Button: {GetButtonStatus(instructionsButton)}");
        Debug.Log($"Quit Button: {GetButtonStatus(quitButton)}");
        Debug.Log($"Back Button: {GetButtonStatus(backToMenuButton)}");
        Debug.Log($"Instructions Panel: {(instructionsPanel != null ? "✅ Found" : "❌ NULL")}");

        // Check scene
        Debug.Log($"Current Scene: {SceneManager.GetActiveScene().name}");
        Debug.Log($"Target Game Scene: {gameSceneName}");
    }

    private string GetButtonStatus(Button button)
    {
        if (button == null) return "❌ NULL";
        if (!button.interactable) return "⚠️ Found but NOT INTERACTABLE";
        if (button.GetComponent<Image>()?.raycastTarget == false) return "⚠️ Found but RAYCAST DISABLED";
        return "✅ Found and Ready";
    }

    private void InitializeMenu()
    {
        // Ensure instructions panel starts hidden
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
        }

        // Auto-fix missing EventSystem
        if (FindObjectOfType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            Debug.LogWarning("Creating missing EventSystem...");
            var eventSystemGO = new GameObject("EventSystem");
            eventSystemGO.AddComponent<UnityEngine.EventSystems.EventSystem>();
            eventSystemGO.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }
    }

    private void SetupButtonListeners()
    {
        Debug.Log("=== SETTING UP BUTTON LISTENERS ===");

        // Clear any existing listeners first
        ClearButtonListeners();

        // Add new listeners with error checking
        AddButtonListener(startButton, OnStartButtonClicked, "Start");
        AddButtonListener(instructionsButton, OnInstructionsButtonClicked, "Instructions");
        AddButtonListener(quitButton, OnQuitButtonClicked, "Quit");
        AddButtonListener(backToMenuButton, OnBackToMenuClicked, "Back");

        Debug.Log("=== BUTTON LISTENER SETUP COMPLETE ===");
    }

    private void ClearButtonListeners()
    {
        startButton?.onClick.RemoveAllListeners();
        instructionsButton?.onClick.RemoveAllListeners();
        quitButton?.onClick.RemoveAllListeners();
        backToMenuButton?.onClick.RemoveAllListeners();
    }

    private void AddButtonListener(Button button, UnityEngine.Events.UnityAction action, string buttonName)
    {
        if (button != null)
        {
            button.onClick.AddListener(action);
            Debug.Log($"✅ {buttonName} button listener added successfully");

            // Ensure button is properly configured
            button.interactable = true;
            var image = button.GetComponent<Image>();
            if (image != null) image.raycastTarget = true;
        }
        else
        {
            Debug.LogError($"❌ {buttonName} button is NULL - CHECK INSPECTOR ASSIGNMENT!");
        }
    }

    // === BUTTON CLICK HANDLERS ===

    public void OnStartButtonClicked()
    {
        Debug.Log("🎮 START BUTTON CLICKED!");
        PlayButtonSound();

        // Simple direct scene loading (SceneTransitionManager dependency removed)
        Debug.Log("Loading scene directly...");
        LoadSceneDirect(gameSceneName);
    }

    public void OnInstructionsButtonClicked()
    {
        Debug.Log("📋 INSTRUCTIONS BUTTON CLICKED!");
        PlayButtonSound();

        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
            Debug.Log("✅ Instructions panel opened");
        }
        else
        {
            Debug.LogError("❌ Instructions panel is NULL!");
        }
    }

    public void OnQuitButtonClicked()
    {
        Debug.Log("🚪 QUIT BUTTON CLICKED!");
        PlayButtonSound();

        StartCoroutine(QuitGameWithDelay());
    }

    public void OnBackToMenuClicked()
    {
        Debug.Log("🔙 BACK BUTTON CLICKED!");
        PlayButtonSound();

        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(false);
            Debug.Log("✅ Instructions panel closed");
        }
        else
        {
            Debug.LogError("❌ Instructions panel is NULL!");
        }
    }

    // === UTILITY METHODS ===

    private void LoadSceneDirect(string sceneName)
    {
        try
        {
            SceneManager.LoadScene(sceneName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load scene '{sceneName}': {e.Message}");
            Debug.LogError("Check that the scene is added to Build Settings!");
        }
    }

    private IEnumerator QuitGameWithDelay()
    {
        yield return new WaitForSeconds(0.1f); // Brief delay for button feedback

#if UNITY_EDITOR
        Debug.Log("Stopping play mode in editor...");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Debug.Log("Quitting application...");
        Application.Quit();
#endif
    }

    private void PlayButtonSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();
        }
    }

    // === DEBUGGING & TESTING ===

    public void TestAllButtons()
    {
        Debug.Log("🧪 TESTING ALL BUTTONS...");
        Debug.Log($"Start Button Clickable: {(startButton?.interactable ?? false)}");
        Debug.Log($"Instructions Button Clickable: {(instructionsButton?.interactable ?? false)}");
        Debug.Log($"Quit Button Clickable: {(quitButton?.interactable ?? false)}");
        Debug.Log($"Back Button Clickable: {(backToMenuButton?.interactable ?? false)}");
    }

    // Call this method if inspector assignments fail
    public void AutoFindButtons()
    {
        Debug.Log("🔍 AUTO-FINDING BUTTONS...");

        Button[] allButtons = FindObjectsOfType<Button>();

        foreach (Button btn in allButtons)
        {
            switch (btn.name.ToLower())
            {
                case "startbutton":
                case "start":
                    startButton = btn;
                    Debug.Log("✅ Found Start button");
                    break;
                case "instructionsbutton":
                case "instructions":
                    instructionsButton = btn;
                    Debug.Log("✅ Found Instructions button");
                    break;
                case "quitbutton":
                case "quit":
                    quitButton = btn;
                    Debug.Log("✅ Found Quit button");
                    break;
                case "backbutton":
                case "back":
                    backToMenuButton = btn;
                    Debug.Log("✅ Found Back button");
                    break;
            }
        }

        // Re-setup listeners after auto-finding
        SetupButtonListeners();
    }

    // Validate the menu setup
    public bool ValidateMenuSetup()
    {
        bool isValid = true;

        if (startButton == null) { Debug.LogError("Start button not assigned!"); isValid = false; }
        if (instructionsButton == null) { Debug.LogError("Instructions button not assigned!"); isValid = false; }
        if (quitButton == null) { Debug.LogError("Quit button not assigned!"); isValid = false; }
        if (backToMenuButton == null) { Debug.LogError("Back button not assigned!"); isValid = false; }
        if (instructionsPanel == null) { Debug.LogError("Instructions panel not assigned!"); isValid = false; }
        if (string.IsNullOrEmpty(gameSceneName)) { Debug.LogError("Game scene name not set!"); isValid = false; }

        return isValid;
    }

    // For testing in inspector
    [ContextMenu("Test Start Button")]
    public void TestStartButton() => OnStartButtonClicked();

    [ContextMenu("Auto Find Buttons")]
    public void TestAutoFind() => AutoFindButtons();

    [ContextMenu("Validate Setup")]
    public void TestValidation() => ValidateMenuSetup();
}