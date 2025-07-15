using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectsManager : MonoBehaviour
{
    [Header("Particle Effect Prefabs")]
    [SerializeField] private ParticleSystem muzzleFlashPrefab;
    [SerializeField] private ParticleSystem bulletImpactPrefab;
    [SerializeField] private ParticleSystem enemyDeathPrefab;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private ParticleSystem bloodHitPrefab;
    [SerializeField] private ParticleSystem sparkHitPrefab;

    [Header("Screen Effects")]
    [SerializeField] private CanvasGroup damageOverlay;
    [SerializeField] private float damageFlashDuration = 0.2f;
    [SerializeField] private Color damageColor = Color.red;

    [Header("Effect Settings")]
    [SerializeField] private int maxActiveParticles = 20;
    [SerializeField] private float effectLifetime = 3f;

    // Singleton pattern
    private static EffectsManager instance;
    public static EffectsManager Instance => instance;

    // Object pooling for performance
    private List<ParticleSystem> activeEffects = new List<ParticleSystem>();
    private Queue<ParticleSystem> particlePool = new Queue<ParticleSystem>();

    private void Awake()
    {
        // Singleton setup
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Create damage overlay if not assigned
        SetupDamageOverlay();

        // Create default particle effects if not assigned
        CreateDefaultParticleEffects();
    }

    private void SetupDamageOverlay()
    {
        if (damageOverlay == null)
        {
            // Create damage overlay canvas
            GameObject overlayGO = new GameObject("DamageOverlay");
            overlayGO.transform.SetParent(transform);

            Canvas canvas = overlayGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000; // Ensure it's on top

            damageOverlay = overlayGO.AddComponent<CanvasGroup>();
            damageOverlay.alpha = 0f;
            damageOverlay.interactable = false;
            damageOverlay.blocksRaycasts = false;

            // Add background image
            UnityEngine.UI.Image background = overlayGO.AddComponent<UnityEngine.UI.Image>();
            background.color = damageColor;

            // Stretch to full screen
            RectTransform rectTransform = overlayGO.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }

    private void CreateDefaultParticleEffects()
    {
        // Create simple particle effects if prefabs aren't assigned  
        if (muzzleFlashPrefab == null) muzzleFlashPrefab = CreateBasicParticleEffect("MuzzleFlash", Color.yellow, 10, 0.1f);
        if (bulletImpactPrefab == null) bulletImpactPrefab = CreateBasicParticleEffect("BulletImpact", Color.white, 15, 0.3f);
        if (enemyDeathPrefab == null) enemyDeathPrefab = CreateBasicParticleEffect("EnemyDeath", Color.red, 25, 0.8f);
        if (explosionPrefab == null) explosionPrefab = CreateBasicParticleEffect("Explosion", new Color(1f, 0.5f, 0f), 30, 1f); // Fixed Color.orange  
        if (bloodHitPrefab == null) bloodHitPrefab = CreateBasicParticleEffect("BloodHit", Color.red, 8, 0.2f);
        if (sparkHitPrefab == null) sparkHitPrefab = CreateBasicParticleEffect("SparkHit", Color.cyan, 12, 0.3f);
    }

    private ParticleSystem CreateBasicParticleEffect(string name, Color color, int particleCount, float duration)
    {
        GameObject effectGO = new GameObject($"Effect_{name}");
        effectGO.transform.SetParent(transform);

        ParticleSystem particles = effectGO.AddComponent<ParticleSystem>();

        // Main module
        var main = particles.main;
        main.startLifetime = duration;
        main.startSpeed = 5f;
        main.maxParticles = particleCount;
        main.startSize = 0.1f;
        main.startColor = color;

        // Emission module
        var emission = particles.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0.0f, (short)particleCount)
        });

        // Shape module
        var shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.2f;

        // Velocity over lifetime
        var velocity = particles.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        velocity.radial = new ParticleSystem.MinMaxCurve(2f, 8f);

        // Size over lifetime (shrink)
        var sizeOverLifetime = particles.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve();
        sizeCurve.AddKey(0f, 1f);
        sizeCurve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        // Color over lifetime (fade out)
        var colorOverLifetime = particles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) }
        );
        colorOverLifetime.color = gradient;

        // Don't play on awake
        main.playOnAwake = false;

        return particles;
    }

    // Public methods for triggering effects

    public static void PlayMuzzleFlash(Vector3 position, float rotation = 0f)
    {
        if (instance != null)
        {
            instance.PlayEffect(instance.muzzleFlashPrefab, position, rotation);
            CameraShake.ShakePlayerShoot();
        }
    }

    public static void PlayBulletImpact(Vector3 position)
    {
        if (instance != null)
        {
            instance.PlayEffect(instance.bulletImpactPrefab, position);
        }
    }

    public static void PlayEnemyDeath(Vector3 position)
    {
        if (instance != null)
        {
            instance.PlayEffect(instance.enemyDeathPrefab, position);
            CameraShake.ShakeEnemyDeath();
        }
    }

    public static void PlayExplosion(Vector3 position)
    {
        if (instance != null)
        {
            instance.PlayEffect(instance.explosionPrefab, position);
            CameraShake.ShakeExplosion();
        }
    }

    public static void PlayHitEffect(Vector3 position, bool isPlayer = false)
    {
        if (instance != null)
        {
            ParticleSystem hitEffect = isPlayer ? instance.bloodHitPrefab : instance.sparkHitPrefab;
            instance.PlayEffect(hitEffect, position);

            if (isPlayer)
            {
                CameraShake.ShakePlayerHit();
                instance.FlashDamageOverlay();
            }
        }
    }

    private void PlayEffect(ParticleSystem effectPrefab, Vector3 position, float rotation = 0f)
    {
        if (effectPrefab == null) return;

        // Clean up old effects if we have too many
        CleanupOldEffects();

        // Get or create particle system instance
        ParticleSystem effect = GetPooledParticleSystem(effectPrefab);

        // Position and play the effect
        effect.transform.position = position;
        effect.transform.rotation = Quaternion.Euler(0, 0, rotation);
        effect.Play();

        // Track active effect
        activeEffects.Add(effect);

        // Auto-cleanup after lifetime
        StartCoroutine(CleanupEffectAfterTime(effect, effectLifetime));
    }

    private ParticleSystem GetPooledParticleSystem(ParticleSystem prefab)
    {
        // Try to get from pool first
        if (particlePool.Count > 0)
        {
            ParticleSystem pooled = particlePool.Dequeue();
            if (pooled != null)
            {
                return pooled;
            }
        }

        // Create new instance
        return Instantiate(prefab, transform);
    }

    private void CleanupOldEffects()
    {
        // Remove finished effects
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            if (activeEffects[i] == null || !activeEffects[i].isPlaying)
            {
                if (activeEffects[i] != null)
                {
                    ReturnToPool(activeEffects[i]);
                }
                activeEffects.RemoveAt(i);
            }
        }

        // Force cleanup if too many active
        while (activeEffects.Count > maxActiveParticles)
        {
            ParticleSystem oldest = activeEffects[0];
            if (oldest != null)
            {
                oldest.Stop();
                ReturnToPool(oldest);
            }
            activeEffects.RemoveAt(0);
        }
    }

    private void ReturnToPool(ParticleSystem effect)
    {
        if (effect != null)
        {
            effect.Stop();
            effect.Clear();
            particlePool.Enqueue(effect);
        }
    }

    private IEnumerator CleanupEffectAfterTime(ParticleSystem effect, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (effect != null && activeEffects.Contains(effect))
        {
            activeEffects.Remove(effect);
            ReturnToPool(effect);
        }
    }

    private void FlashDamageOverlay()
    {
        if (damageOverlay != null)
        {
            StartCoroutine(DamageFlashCoroutine());
        }
    }

    private IEnumerator DamageFlashCoroutine()
    {
        // Fade in quickly
        float elapsed = 0f;
        float fadeInTime = damageFlashDuration * 0.2f;

        while (elapsed < fadeInTime)
        {
            damageOverlay.alpha = Mathf.Lerp(0f, 0.3f, elapsed / fadeInTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Fade out slowly
        elapsed = 0f;
        float fadeOutTime = damageFlashDuration * 0.8f;

        while (elapsed < fadeOutTime)
        {
            damageOverlay.alpha = Mathf.Lerp(0.3f, 0f, elapsed / fadeOutTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        damageOverlay.alpha = 0f;
    }
}