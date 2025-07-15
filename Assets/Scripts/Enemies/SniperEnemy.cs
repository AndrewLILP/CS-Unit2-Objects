// SniperEnemy.cs - High accuracy, long range enemy with aiming time and repositioning
using UnityEngine;

public class SniperEnemy : ShooterEnemy
{
    [Header("Sniper Specific")]
    [SerializeField] private float inaccuracyAngle = 1f;
    [SerializeField] private float aimingTime = 1.5f;
    [SerializeField] private bool requiresLineOfSight = true;

    // ==================== NEW: REPOSITIONING SYSTEM ====================
    [Header("Repositioning Behavior")]
    [SerializeField] private int shotsBeforeReposition = 3;
    [SerializeField] private float repositionCooldown = 2f;
    [SerializeField] private float panicRange = 4f; // Distance that triggers panic mode
    [SerializeField] private float panicModeSpeedMultiplier = 1.5f;

    private int shotsTaken = 0;
    private bool isRepositioning = false;
    private float repositionTimer = 0f;
    private bool isInPanicMode = false;
    private Vector3 repositionTarget;
    // ====================================================================

    private bool isAiming = false;
    private float aimStartTime = 0f;
    private bool hasLineOfSight = false;

    protected override void Start()
    {
        base.Start();
        // Snipers are fragile but deadly
        health = new Health(1, 0, 1);

        // Set sniper-specific ranges using inherited fields
        preferredDistance = 12f;
        minimumDistance = 8f;
        maxDistance = 15f;

        // High damage, slow fire rate
        bulletDamage = 3f;
        fireRate = 0.33f; // One shot every 3 seconds
        bulletSpeed = 20f;
        shootingRange = 12f;
    }

    protected override void Update()
    {
        base.Update();

        if (requiresLineOfSight)
        {
            CheckLineOfSight();
        }

        // ==================== NEW: REPOSITIONING AND PANIC LOGIC ====================
        if (target != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            // Check for panic mode
            CheckPanicMode(distanceToTarget);

            // Handle repositioning
            HandleRepositioning();
        }
        // ==============================================================================
    }

    // ==================== NEW: PANIC MODE SYSTEM ====================
    private void CheckPanicMode(float distanceToTarget)
    {
        if (distanceToTarget <= panicRange && !isInPanicMode)
        {
            EnterPanicMode();
        }
        else if (distanceToTarget > panicRange && isInPanicMode)
        {
            ExitPanicMode();
        }
    }

    private void EnterPanicMode()
    {
        isInPanicMode = true;
        speed *= panicModeSpeedMultiplier;
        fireRate *= 1.5f; // Faster but less accurate shooting
        inaccuracyAngle *= 2f; // Less accurate when panicking
        Debug.Log("Sniper entering panic mode!");
    }

    private void ExitPanicMode()
    {
        isInPanicMode = false;
        speed /= panicModeSpeedMultiplier;
        fireRate /= 1.5f;
        inaccuracyAngle /= 2f;
        Debug.Log("Sniper exiting panic mode");
    }
    // ================================================================

    // ==================== NEW: REPOSITIONING SYSTEM ====================
    private void HandleRepositioning()
    {
        // Check if we need to reposition
        if (!isRepositioning && shotsTaken >= shotsBeforeReposition)
        {
            StartRepositioning();
        }

        // Handle repositioning movement
        if (isRepositioning)
        {
            repositionTimer += Time.deltaTime;

            // Move towards reposition target
            Vector2 direction = (repositionTarget - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);

            // Check if we've reached the target or timeout
            if (Vector2.Distance(transform.position, repositionTarget) < 1f || repositionTimer >= repositionCooldown)
            {
                FinishRepositioning();
            }
        }
    }

    private void StartRepositioning()
    {
        isRepositioning = true;
        repositionTimer = 0f;
        shotsTaken = 0;
        isAiming = false; // Stop aiming during reposition

        // Find a new position (simple random offset for now)
        Vector2 randomOffset = Random.insideUnitCircle * 5f;
        repositionTarget = transform.position + (Vector3)randomOffset;

        Debug.Log("Sniper repositioning...");
    }

    private void FinishRepositioning()
    {
        isRepositioning = false;
        repositionTimer = 0f;
        Debug.Log("Sniper repositioning complete");
    }
    // ================================================================

    protected override bool CanShootAtTarget(float distanceToTarget)
    {
        bool baseCanShoot = base.CanShootAtTarget(distanceToTarget);
        return baseCanShoot && (!requiresLineOfSight || hasLineOfSight) && !isRepositioning;
    }

    protected override void HandleShooting(float distanceToTarget)
    {
        if (!CanShootAtTarget(distanceToTarget))
        {
            // Reset aiming if can't shoot
            if (isAiming)
            {
                isAiming = false;
                Debug.Log("Sniper: Lost target, stopping aim");
            }
            return;
        }

        if (!isAiming)
        {
            StartAiming();
        }
        else if (Time.time >= aimStartTime + aimingTime)
        {
            if (IsReadyToShoot())
            {
                PerformShoot();
                isAiming = false;
                shotsTaken++; // NEW: Track shots for repositioning
            }
        }
    }

    private void StartAiming()
    {
        isAiming = true;
        aimStartTime = Time.time;
        Debug.Log("Sniper: Aiming...");
    }

    protected override float GetInaccuracyAngle()
    {
        return isAiming ? inaccuracyAngle : inaccuracyAngle * 3f; // Less accurate if not fully aimed
    }

    private void CheckLineOfSight()
    {
        if (target == null)
        {
            hasLineOfSight = false;
            return;
        }

        Vector2 directionToTarget = (target.position - transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget);

        hasLineOfSight = hit.collider == null || hit.collider.CompareTag("Player");

        // Debug visualization
        Debug.DrawRay(transform.position, directionToTarget * distanceToTarget,
                     hasLineOfSight ? Color.green : Color.red);
    }

    protected override void HandleMovement(float distanceToTarget)
    {
        // Don't use normal movement during repositioning
        if (isRepositioning) return;

        // Snipers move more carefully to maintain optimal range
        if (distanceToTarget > maxDistance)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (distanceToTarget < minimumDistance)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if (distanceToTarget > preferredDistance)
        {
            // Move slowly to preferred position
            transform.Translate(Vector2.right * speed * 0.3f * Time.deltaTime);
        }
    }

    public override void GetDamage(float damage)
    {
        // Interrupt aiming when taking damage
        isAiming = false;

        // Force immediate repositioning if not already repositioning
        if (!isRepositioning)
        {
            shotsTaken = shotsBeforeReposition; // Force reposition on next update
        }

        base.GetDamage(damage);
    }

    protected override void OnShootEffects()
    {
        Debug.Log("Sniper: High-powered shot fired!");
        // Add sniper-specific effects like scope glint, loud sound, etc.
    }

    public void ConfigureSniper(float _aimingTime, float _inaccuracyAngle, float _damage)
    {
        aimingTime = _aimingTime;
        inaccuracyAngle = _inaccuracyAngle;
        bulletDamage = _damage;
    }

    // NEW: Enhanced configuration method
    public void ConfigureSniperAdvanced(float _aimingTime, float _inaccuracyAngle, float _damage, int _shotsBeforeReposition)
    {
        ConfigureSniper(_aimingTime, _inaccuracyAngle, _damage);
        shotsBeforeReposition = _shotsBeforeReposition;
    }

    // Backwards compatibility method for existing GameManager code
    public void SetSniperEnemy(float _aimingTime, float _inaccuracyAngle)
    {
        ConfigureSniper(_aimingTime, _inaccuracyAngle, bulletDamage);
    }
}