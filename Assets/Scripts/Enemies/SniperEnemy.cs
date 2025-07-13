using UnityEngine;

/// <summary>
/// SniperEnemy class inherits from ShooterEnemy.
/// Stays far from player, shoots with high accuracy but low rate.
/// Maintains line of sight once player is found.
/// </summary>
public class SniperEnemy : ShooterEnemy
{
    [Header("Sniper Distance Settings")]
    [SerializeField] private float preferredDistance = 12f; // Ideal distance to maintain from player
    [SerializeField] private float minimumDistance = 8f;    // Don't get closer than this
    [SerializeField] private float maxDistance = 15f;       // Don't get farther than this

    [Header("Sniper Combat Settings")]
    [SerializeField] private Bullet BulletPrefab;           // Bullet prefab for the sniper
    [SerializeField] private Transform firePoint;           // Spawn point for the bullet
    [SerializeField] private float fireRate = 0.33f;        // 1 shot every 3 seconds (1/3 = 0.33)
    [SerializeField] private float inaccuracyAngle = 1f;     // Very high accuracy (low inaccuracy)
    [SerializeField] private float shootingRange = 12f;     // Long range shooting capability
    [SerializeField] private float sniperDamage = 3f;       // High damage per shot
    [SerializeField] private float bulletSpeed = 20f;       // Fast bullet speed

    private float nextFireTime = 0f;        // Time when the enemy can fire again
    private bool hasLineOfSight = false;    // Whether sniper can see the player
    private bool isAiming = false;          // Whether sniper is currently aiming
    private float aimingTime = 1f;          // Time spent aiming before shooting
    private float aimStartTime = 0f;        // When aiming started

    protected override void Start()
    {
        base.Start();
        // Snipers are fragile but dangerous
        health = new Health(1, 0, 1);
    }

    protected override void Update()
    {
        if (target == null)
        {
            Debug.Log("SniperEnemy: No target found!");
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // Debug information
        Debug.Log($"SniperEnemy: Distance to target: {distanceToTarget:F1}, Shooting range: {shootingRange}, Has LOS: {hasLineOfSight}, Can fire: {Time.time >= nextFireTime}");

        // Check line of sight to player
        CheckLineOfSight();

        // ALWAYS face the player for accurate tracking
        FaceTarget();

        // Handle movement based on distance to maintain optimal range
        HandleMovement(distanceToTarget);

        // Handle shooting logic with line of sight requirement
        HandleShooting(distanceToTarget);
    }

    private void FaceTarget()
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void HandleMovement(float distanceToTarget)
    {
        // Sniper tries to maintain optimal distance for accuracy
        if (distanceToTarget > maxDistance)
        {
            // Too far - move toward player to get in effective range
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (distanceToTarget < minimumDistance)
        {
            // Too close - back away to maintain safe sniping distance
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
        else if (distanceToTarget > preferredDistance && distanceToTarget <= maxDistance)
        {
            // In acceptable range but not at preferred distance - move slowly to ideal position
            transform.Translate(Vector2.right * speed * 0.3f * Time.deltaTime);
        }
        // If at ideal distance (preferredDistance), stay put and focus on accuracy
    }

    private void HandleShooting(float distanceToTarget)
    {
        // Only initiate shooting sequence if conditions are met
        // Temporarily removed hasLineOfSight check for debugging
        if (distanceToTarget <= shootingRange && Time.time >= nextFireTime)
        {
            Debug.Log("SniperEnemy: Shooting conditions met!");
            if (!isAiming)
            {
                // Start aiming sequence
                Debug.Log("SniperEnemy: Starting to aim...");
                StartAiming();
            }
            else if (Time.time >= aimStartTime + aimingTime)
            {
                // Aiming complete - fire the shot
                Debug.Log("SniperEnemy: Firing shot!");
                Shoot();
                isAiming = false;
            }
            else
            {
                Debug.Log($"SniperEnemy: Still aiming... {(aimStartTime + aimingTime - Time.time):F1}s left");
            }
        }
        else
        {
            // Reset aiming if conditions are no longer met
            if (isAiming)
            {
                Debug.Log("SniperEnemy: Shooting conditions no longer met, stopping aim");
            }
            isAiming = false;
        }
    }

    private void StartAiming()
    {
        isAiming = true;
        aimStartTime = Time.time;
        Debug.Log("Sniper is aiming...");
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

        // Cast a ray to check for obstacles between sniper and player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget);

        // If raycast hits the player directly or nothing (clear path), we have line of sight
        if (hit.collider == null || hit.collider.CompareTag("Player"))
        {
            hasLineOfSight = true;
        }
        else
        {
            hasLineOfSight = false;
        }

        // Optional: Draw debug line to visualize line of sight in Scene view
        Debug.DrawRay(transform.position, directionToTarget * distanceToTarget,
                     hasLineOfSight ? Color.green : Color.red);
    }

    public override void Shoot()
    {
        Debug.Log("SniperEnemy: Shoot() method called!");

        // Check if required components are assigned
        if (BulletPrefab == null)
        {
            Debug.LogError("SniperEnemy: BulletPrefab is not assigned!");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("SniperEnemy: FirePoint is not assigned!");
            return;
        }

        // Set next fire time based on fire rate (3 second cooldown)
        nextFireTime = Time.time + (1f / fireRate);
        Debug.Log($"SniperEnemy: Next fire time set to {nextFireTime:F1}");

        // Apply minimal random angle for slight inaccuracy (snipers are very accurate)
        float randomAngle = Random.Range(-inaccuracyAngle, inaccuracyAngle);

        // Calculate bullet rotation with minimal inaccuracy
        Quaternion bulletRotation = Quaternion.Euler(0, 0, (transform.eulerAngles.z + randomAngle - 90));

        // Instantiate and configure the bullet
        Debug.Log($"SniperEnemy: Instantiating bullet at {firePoint.position}");
        Bullet bullet = Instantiate(BulletPrefab, firePoint.position, bulletRotation);
        bullet.SetBullet(sniperDamage, "Player", bulletSpeed);

        Debug.Log($"Sniper fired! Damage: {sniperDamage}");
    }

    public override void GetDamage(float damage)
    {
        // Snipers might reposition when taking damage
        isAiming = false; // Interrupt aiming if hit
        base.GetDamage(damage);
    }

    /// <summary>
    /// Configuration method to set sniper parameters from GameManager
    /// </summary>
    public void SetSniperEnemy(float _fireRate, float _inaccuracyAngle, float _sniperDamage, float _aimingTime = 1f)
    {
        fireRate = _fireRate;
        inaccuracyAngle = _inaccuracyAngle;
        sniperDamage = _sniperDamage;
        aimingTime = _aimingTime;
    }

    
}