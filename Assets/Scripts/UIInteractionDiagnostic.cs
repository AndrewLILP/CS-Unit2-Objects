using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIInteractionDiagnostic : MonoBehaviour
{
    [Header("Click this button to run diagnostics")]
    public bool runDiagnostics = false;

    private void Update()
    {
        if (runDiagnostics)
        {
            runDiagnostics = false;
            RunFullDiagnostics();
        }

        // Real-time click detection
        if (Input.GetMouseButtonDown(0))
        {
            DetectMouseClick();
        }
    }

    public void RunFullDiagnostics()
    {
        Debug.Log("🔍 === FULL UI INTERACTION DIAGNOSTICS ===");

        CheckCanvasSettings();
        CheckCanvasGroups();
        CheckButtonHierarchy();
        CheckForBlockingElements();
        CheckInputSystem();

        Debug.Log("🔍 === DIAGNOSTICS COMPLETE ===");
    }

    private void CheckCanvasSettings()
    {
        Debug.Log("\n📋 CANVAS SETTINGS CHECK:");

        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            Debug.Log($"Canvas: {canvas.name}");
            Debug.Log($"  Render Mode: {canvas.renderMode}");
            Debug.Log($"  Sort Order: {canvas.sortingOrder}");
            Debug.Log($"  Override Sorting: {canvas.overrideSorting}");

            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler != null)
            {
                Debug.Log($"  UI Scale Mode: {scaler.uiScaleMode}");
                Debug.Log($"  Reference Resolution: {scaler.referenceResolution}");
            }

            GraphicRaycaster raycaster = canvas.GetComponent<GraphicRaycaster>();
            if (raycaster != null)
            {
                Debug.Log($"  Graphic Raycaster: ✅ Found");
                Debug.Log($"  Ignore Reversed Graphics: {raycaster.ignoreReversedGraphics}");
                Debug.Log($"  Blocking Objects: {raycaster.blockingObjects}");
            }
            else
            {
                Debug.LogError($"  Graphic Raycaster: ❌ MISSING!");
            }
        }
    }

    private void CheckCanvasGroups()
    {
        Debug.Log("\n🚫 CANVAS GROUP CHECK:");

        CanvasGroup[] canvasGroups = FindObjectsOfType<CanvasGroup>();
        foreach (CanvasGroup group in canvasGroups)
        {
            Debug.Log($"Canvas Group: {group.name}");
            Debug.Log($"  Interactable: {(group.interactable ? "✅" : "❌ BLOCKED!")}");
            Debug.Log($"  Blocks Raycasts: {(group.blocksRaycasts ? "✅" : "❌ BLOCKED!")}");
            Debug.Log($"  Alpha: {group.alpha}");

            if (!group.interactable || !group.blocksRaycasts)
            {
                Debug.LogError($"❌ PROBLEM FOUND: {group.name} is blocking interactions!");
            }
        }

        if (canvasGroups.Length == 0)
        {
            Debug.Log("No Canvas Groups found - this is normal");
        }
    }

    private void CheckButtonHierarchy()
    {
        Debug.Log("\n🔘 BUTTON HIERARCHY CHECK:");

        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            Debug.Log($"\nButton: {button.name}");
            Debug.Log($"  Interactable: {(button.interactable ? "✅" : "❌")}");
            Debug.Log($"  Active: {(button.gameObject.activeInHierarchy ? "✅" : "❌")}");

            Image buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
            {
                Debug.Log($"  Image Raycast Target: {(buttonImage.raycastTarget ? "✅" : "❌")}");
                Debug.Log($"  Image Color Alpha: {buttonImage.color.a}");
            }

            // Check parent hierarchy for blocking elements
            Transform parent = button.transform.parent;
            int depth = 0;
            while (parent != null && depth < 10)
            {
                CanvasGroup parentGroup = parent.GetComponent<CanvasGroup>();
                if (parentGroup != null)
                {
                    Debug.Log($"  Parent CanvasGroup ({parent.name}): Interactable={parentGroup.interactable}, BlocksRaycasts={parentGroup.blocksRaycasts}");
                    if (!parentGroup.interactable || !parentGroup.blocksRaycasts)
                    {
                        Debug.LogError($"❌ PARENT BLOCKING: {parent.name} is blocking {button.name}!");
                    }
                }
                parent = parent.parent;
                depth++;
            }
        }
    }

    private void CheckForBlockingElements()
    {
        Debug.Log("\n🛡️ BLOCKING ELEMENTS CHECK:");

        // Check for invisible blocking images
        Image[] allImages = FindObjectsOfType<Image>();
        foreach (Image img in allImages)
        {
            if (img.raycastTarget && img.color.a < 0.01f)
            {
                Debug.LogWarning($"⚠️ INVISIBLE BLOCKER: {img.name} has raycastTarget=true but is nearly transparent!");
            }
        }

        // Check for objects on higher sorting layers
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            Debug.Log($"Canvas '{canvas.name}' - Sort Order: {canvas.sortingOrder}");
        }
    }

    private void CheckInputSystem()
    {
        Debug.Log("\n🎮 INPUT SYSTEM CHECK:");

        EventSystem eventSystem = FindObjectOfType<EventSystem>();
        if (eventSystem != null)
        {
            Debug.Log($"EventSystem: ✅ Found - {eventSystem.name}");
            Debug.Log($"  Current Selected: {(eventSystem.currentSelectedGameObject?.name ?? "None")}");

            BaseInputModule inputModule = eventSystem.currentInputModule;
            if (inputModule != null)
            {
                Debug.Log($"  Input Module: {inputModule.GetType().Name}");
            }
            else
            {
                Debug.LogError("❌ NO INPUT MODULE FOUND!");
            }
        }
        else
        {
            Debug.LogError("❌ NO EVENT SYSTEM FOUND!");
        }
    }

    private void DetectMouseClick()
    {
        Vector2 mousePos = Input.mousePosition;

        // Raycast to see what's under the mouse
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePos;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        Debug.Log($"🖱️ MOUSE CLICK at {mousePos}:");
        Debug.Log($"   Raycast found {results.Count} objects:");

        foreach (RaycastResult result in results)
        {
            Debug.Log($"   - {result.gameObject.name} (depth: {result.depth})");

            Button button = result.gameObject.GetComponent<Button>();
            if (button != null)
            {
                Debug.Log($"     ✅ This is a BUTTON! Interactable: {button.interactable}");
            }
        }

        if (results.Count == 0)
        {
            Debug.LogError("❌ MOUSE RAYCAST FOUND NOTHING! This means UI is not receiving input!");
        }
    }

    // Quick fix methods
    [ContextMenu("Fix Common Issues")]
    public void FixCommonIssues()
    {
        Debug.Log("🔧 APPLYING COMMON FIXES...");

        // Fix Canvas Groups
        CanvasGroup[] groups = FindObjectsOfType<CanvasGroup>();
        foreach (CanvasGroup group in groups)
        {
            group.interactable = true;
            group.blocksRaycasts = true;
            Debug.Log($"✅ Fixed Canvas Group: {group.name}");
        }

        // Fix Button Images
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.interactable = true;
            Image img = button.GetComponent<Image>();
            if (img != null)
            {
                img.raycastTarget = true;
            }
            Debug.Log($"✅ Fixed Button: {button.name}");
        }

        // Ensure GraphicRaycaster on Canvas
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.GetComponent<GraphicRaycaster>() == null)
            {
                canvas.gameObject.AddComponent<GraphicRaycaster>();
                Debug.Log($"✅ Added GraphicRaycaster to: {canvas.name}");
            }
        }

        Debug.Log("🔧 COMMON FIXES APPLIED!");
    }

    [ContextMenu("Test Button Clicks")]
    public void TestButtonClicks()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            Debug.Log($"🧪 Testing button: {button.name}");
            button.onClick.Invoke();
        }
    }
}