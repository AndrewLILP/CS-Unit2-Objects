using UnityEngine;

public class ExploderEnemy : Enemy
{
    [Header("Explosion Settings")]
    [SerializeField] private float explosionRange = 2f;
    [SerializeField] private float explosionDamage = 2f;
    [SerializeField] private float fuseTime = 1f; // Time before explosion after trigger
    [SerializeField] private float chainReactionRange = 3f; // Range to trigger other exploders

    [Header("Movement Settings")]
    [SerializeField] private float panicSpeedMultiplier = 1.5f; // Speed boost when fuse is lit

    [Header("Visual/Audio Feedback")]
    [SerializeField] private float blinkSpeed = 5f; // How fast to blink when about to explode

    private bool fuseIsLit = false;
    private float fuseTimer = 0f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isBlinking = false;

    protected override void Start()
    {
        base.Start();
        health = new Health(1, 0, 1); // Fragile but dangerous

        // Get sprite renderer for visual feedback
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Check if we should start the fuse
        if (!fuseIsLit && distanceToTarget <= explosionRange)
        {
            LightFuse();
        }

        // Handle fuse countdown
        if (fuseIsLit)
        {
            HandleFuseCountdown();
        }
    }

    private void LightFuse()
    {
        fuseIsLit = true;
        fuseTimer = 0f;

        // Speed boost when fuse is lit (panic mode)
        speed *= panicSpeedMultiplier;

        // Start visual feedback
        StartBlinking();

        Debug.Log("Exploder fuse lit! Exploding in " + fuseTime + " seconds!");
    }

    private void HandleFuseCountdown()
    {
        fuseTimer += Time.deltaTime;

        if (fuseTimer >= fuseTime)
        {
            Explode();
        }
    }

    private void StartBlinking()
    {
        if (!isBlinking)
        {
            isBlinking = true;
            InvokeRepeating(nameof(BlinkEffect), 0f, 1f / blinkSpeed);
        }
    }

    private void BlinkEffect()
    {
        if (spriteRenderer != null)
        {
            // Alternate between red and original color
            spriteRenderer.color = spriteRenderer.color == originalColor ? Color.red : originalColor;
        }
    }

    private void Explode()
    {
        Debug.Log("BOOM! Exploder detonated!");

        // Damage player if in range
        if (target != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, target.position);
            if (distanceToPlayer <= explosionRange)
            {
                var damageable = target.GetComponent<IDamagable>();
                if (damageable != null)
                {
                    damageable.GetDamage(explosionDamage);
                    Debug.Log($"Player hit by explosion for {explosionDamage} damage!");
                }
            }
        }

        // Chain reaction - trigger nearby exploders
        TriggerChainReaction();

        // TODO: Add explosion particle effect here
        // TODO: Add explosion sound effect here

        // Destroy this exploder
        Die();
    }

    private void TriggerChainReaction()
    {
        // Find other exploders in chain reaction range
        Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, chainReactionRange);

        foreach (Collider2D collider in nearbyColliders)
        {
            ExploderEnemy otherExploder = collider.GetComponent<ExploderEnemy>();
            if (otherExploder != null && otherExploder != this && !otherExploder.fuseIsLit)
            {
                Debug.Log("Chain reaction triggered!");
                otherExploder.LightFuse();
            }
        }
    }

    public override void Attack(float interval)
    {
        // Exploders don't attack - they explode
        // This method is required by the abstract base class
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);

        // If damaged while fuse is lit, explode immediately (panic mode)
        if (fuseIsLit && health.GetHealth() > 0)
        {
            Debug.Log("Exploder damaged while fuse lit - immediate explosion!");
            fuseTimer = fuseTime; // Force immediate explosion on next update
        }
    }

    public override void Die()
    {
        // Stop blinking before destruction
        if (isBlinking)
        {
            CancelInvoke(nameof(BlinkEffect));
        }

        base.Die();
    }

    // Configuration method for GameManager
    public void ConfigureExploder(float _explosionRange, float _explosionDamage, float _fuseTime)
    {
        explosionRange = _explosionRange;
        explosionDamage = _explosionDamage;
        fuseTime = _fuseTime;
    }

}