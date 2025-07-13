using UnityEngine;

/// <summary>
/// MachineGunEnemy class inherits from ShooterEnemy.
/// it needs to cool down after shooting.
/// </summary>

public class MachineGunEnenmy : ShooterEnemy // : MonoBehaviour via ShooterEnemy Enemyand PlayableObject
{
    [SerializeField] private float preferredDistance = 6f; // Ideal distance to maintain
    [SerializeField] private float minimumDistance = 4f;   // Don't get closer than this

    [SerializeField] private Bullet BulletPrefab; // Bullet prefab for the shooter enemy
    [SerializeField] private Transform firePoint; // Spawn point for the bullet
    [SerializeField] private float fireRate = 5f; // Speed of the bullet
    [SerializeField] private float inaccuracyAngle = 15f; // Inaccuracy angle in degrees
    [SerializeField] private float shootingRange = 8f; // Range within which the enemy can shoot
    private float nextFireTime = 0f; // Time when the enemy can fire again
    private bool isShooting = false; // Flag to check if the enemy is currently shooting

    protected override void Start()
    {
        base.Start();
        // Initialize the ShooterEnemy's health
        health = new Health(1, 0, 1); // set health for Shooter 
    }

    protected override void Update()
    {
        if (target == null)
        {
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // ALWAYS face the player first
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Movement logic based on distance
        if (distanceToTarget > preferredDistance)
        {
            // Too far - move toward player (already facing correct direction)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else if (distanceToTarget < minimumDistance)
        {
            // Too close - move away from player
            transform.Translate(Vector2.left * speed * Time.deltaTime); // Move backward
        }
        // If in ideal range (between min and preferred), don't move but keep facing player

        // Shooting logic (only when in range)
        if (distanceToTarget <= shootingRange && Time.time >= nextFireTime)
        {
            Shoot();
        }
    }

    public override void Shoot()
    {
        isShooting = true;
        nextFireTime = Time.time + (1f / fireRate);

        // Calculate random angle for inaccuracy
        float randomAngle = Random.Range(-inaccuracyAngle, inaccuracyAngle);

        // Use the enemy's current rotation + random inaccuracy
        Quaternion bulletRotation = Quaternion.Euler(0, 0, (transform.eulerAngles.z + randomAngle -90));

        Bullet bullet = Instantiate(BulletPrefab, firePoint.position, bulletRotation);
        bullet.SetBullet(1f, "Player", 10f);
        isShooting = false;
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }

    public void SetMachineGunEnenmy(float _fireRate, float _inaccuracyAngle)
    {
        fireRate = _fireRate;
        inaccuracyAngle = _inaccuracyAngle;
        // Set the fire rate and inaccuracy angle for the machine gun enemy
    }

}
