// ShooterEnemy.cs - Base class for all shooting enemies
using UnityEngine;

public abstract class ShooterEnemy : Enemy
{
    [Header("Shooting Configuration")]
    [SerializeField] protected Bullet bulletPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float fireRate = 1f;
    [SerializeField] protected float shootingRange = 8f;
    [SerializeField] protected float bulletSpeed = 10f;
    [SerializeField] protected float bulletDamage = 1f;
    [SerializeField] protected string targetTag = "Player";

    [Header("Movement Configuration")]
    [SerializeField] protected float preferredDistance = 6f;
    [SerializeField] protected float minimumDistance = 3f;
    [SerializeField] protected float maxDistance = 10f;

    protected float nextFireTime = 0f;
    protected bool canShoot = true;

    protected override void Start()
    {
        base.Start();
        ValidateShootingComponents();
    }

    protected override void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Always face the target
        FaceTarget();

        // Handle movement based on distance preferences
        HandleMovement(distanceToTarget);

        // Handle shooting logic
        HandleShooting(distanceToTarget);
    }

    protected virtual void FaceTarget()
    {
        if (target == null) return;

        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected virtual void HandleMovement(float distanceToTarget)
    {
        // Default movement logic - can be overridden by derived classes
        if (distanceToTarget > preferredDistance)
        {
            // Move toward target
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (distanceToTarget < minimumDistance)
        {
            // Move away from target
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    protected virtual void HandleShooting(float distanceToTarget)
    {
        if (CanShootAtTarget(distanceToTarget))
        {
            if (IsReadyToShoot())
            {
                PerformShoot();
            }
        }
    }

    protected virtual bool CanShootAtTarget(float distanceToTarget)
    {
        return distanceToTarget <= shootingRange && canShoot;
    }

    protected virtual bool IsReadyToShoot()
    {
        return Time.time >= nextFireTime;
    }

    protected virtual void PerformShoot()
    {
        Shoot();
        SetNextFireTime();
    }

    public override void Shoot()
    {
        if (!ValidateShootingComponents()) return;

        // Create bullet with rotation and inaccuracy
        Quaternion bulletRotation = CalculateBulletRotation();
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);

        // Configure bullet
        bullet.SetBullet(bulletDamage, targetTag, bulletSpeed);

        // Play shooting effects if needed
        OnShootEffects();
    }

    protected virtual Quaternion CalculateBulletRotation()
    {
        float inaccuracy = GetInaccuracyAngle();
        float randomAngle = Random.Range(-inaccuracy, inaccuracy);
        return Quaternion.Euler(0, 0, transform.eulerAngles.z + randomAngle - 90);
    }

    protected virtual float GetInaccuracyAngle()
    {
        return 5f; // Default inaccuracy
    }

    protected virtual void SetNextFireTime()
    {
        nextFireTime = Time.time + (1f / fireRate);
    }

    protected virtual void OnShootEffects()
    {
        // Override in derived classes for muzzle flash, sound, etc.
    }

    protected virtual bool ValidateShootingComponents()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError($"{GetType().Name}: BulletPrefab is not assigned!");
            return false;
        }

        if (firePoint == null)
        {
            Debug.LogError($"{GetType().Name}: FirePoint is not assigned!");
            return false;
        }

        return true;
    }

    // Configuration method for GameManager
    public virtual void ConfigureShooter(float damage, float range, float rate, float speed)
    {
        bulletDamage = damage;
        shootingRange = range;
        fireRate = rate;
        bulletSpeed = speed;
    }
}