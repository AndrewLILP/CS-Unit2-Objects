// SniperEnemy.cs - High accuracy, long range enemy with aiming time
using UnityEngine;

public class SniperEnemy : ShooterEnemy
{
    [Header("Sniper Specific")]
    [SerializeField] private float inaccuracyAngle = 1f;
    [SerializeField] private float aimingTime = 1.5f;
    [SerializeField] private bool requiresLineOfSight = true;

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
    }

    protected override bool CanShootAtTarget(float distanceToTarget)
    {
        bool baseCanShoot = base.CanShootAtTarget(distanceToTarget);
        return baseCanShoot && (!requiresLineOfSight || hasLineOfSight);
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

    // Backwards compatibility method for existing GameManager code
    public void SetSniperEnemy(float _aimingTime, float _inaccuracyAngle)
    {
        ConfigureSniper(_aimingTime, _inaccuracyAngle, bulletDamage);
    }
}