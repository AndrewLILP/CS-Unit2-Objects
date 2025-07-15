using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float maxShakeIntensity = 1f;
    [SerializeField] private float maxShakeDuration = 0.5f;

    private Camera targetCamera;
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    // Singleton pattern for easy access
    private static CameraShake instance;
    public static CameraShake Instance => instance;

    private void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Get camera component
        targetCamera = GetComponent<Camera>();
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera != null)
        {
            originalPosition = targetCamera.transform.localPosition;
        }
    }

    private void Start()
    {
        // Store original position after any camera setup is complete
        if (targetCamera != null)
        {
            originalPosition = targetCamera.transform.localPosition;
        }
    }

    /// <summary>
    /// Trigger camera shake with specified intensity and duration
    /// </summary>
    /// <param name="intensity">Shake strength (0-1, will be scaled by maxShakeIntensity)</param>
    /// <param name="duration">How long to shake (will be scaled by maxShakeDuration)</param>
    public static void Shake(float intensity, float duration)
    {
        if (instance != null)
        {
            instance.TriggerShake(intensity, duration);
        }
    }

    /// <summary>
    /// Predefined shake types for common scenarios
    /// </summary>
    public static void ShakeLight() => Shake(0.3f, 0.2f);
    public static void ShakeMedium() => Shake(0.6f, 0.4f);
    public static void ShakeHeavy() => Shake(1f, 0.6f);

    /// <summary>
    /// Weapon-specific shake effects
    /// </summary>
    public static void ShakePlayerShoot() => Shake(0.2f, 0.1f);
    public static void ShakePlayerHit() => Shake(0.8f, 0.3f);
    public static void ShakeEnemyDeath() => Shake(0.4f, 0.2f);
    public static void ShakeExplosion() => Shake(1f, 0.5f);

    private void TriggerShake(float intensity, float duration)
    {
        if (targetCamera == null) return;

        // Stop any existing shake
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        // Start new shake
        shakeCoroutine = StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Calculate diminishing intensity over time
            float currentIntensity = Mathf.Lerp(intensity * maxShakeIntensity, 0f, elapsed / duration);

            // Generate random shake offset
            Vector3 shakeOffset = Random.insideUnitCircle * currentIntensity;
            shakeOffset.z = 0; // Keep Z position unchanged

            // Apply shake to camera
            targetCamera.transform.localPosition = originalPosition + shakeOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Return to original position
        targetCamera.transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }

    /// <summary>
    /// Stop any ongoing shake and return to original position
    /// </summary>
    public static void StopShake()
    {
        if (instance != null && instance.shakeCoroutine != null)
        {
            instance.StopCoroutine(instance.shakeCoroutine);
            instance.targetCamera.transform.localPosition = instance.originalPosition;
            instance.shakeCoroutine = null;
        }
    }

    /// <summary>
    /// Update original position (call this if camera moves permanently)
    /// </summary>
    public static void UpdateOriginalPosition()
    {
        if (instance != null && instance.targetCamera != null)
        {
            instance.originalPosition = instance.targetCamera.transform.localPosition;
        }
    }
}