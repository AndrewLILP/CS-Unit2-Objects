// MachineGunEnemy.cs - Fast firing, inaccurate enemy
using UnityEngine;

public class MachineGunEnemy : ShooterEnemy
{
    [Header("Machine Gun Specific")]
    [SerializeField] private float inaccuracyAngle = 15f;
    [SerializeField] private float burstDuration = 3f;
    [SerializeField] private float burstCooldown = 2f;

    private bool isBursting = false;
    private float burstStartTime = 0f;
    private float burstEndTime = 0f;

    protected override void Start()
    {
        base.Start();
        // Machine gunners have moderate health
        health = new Health(2, 0, 2);

        // Set default values using inherited fields
        preferredDistance = 6f;
        minimumDistance = 4f;
        maxDistance = 8f;
        fireRate = 5f;
        shootingRange = 8f;
        bulletDamage = 1f;
        bulletSpeed = 10f;
    }

    protected override void HandleShooting(float distanceToTarget)
    {
        if (!CanShootAtTarget(distanceToTarget)) return;

        HandleBurstFiring();
    }

    private void HandleBurstFiring()
    {
        if (!isBursting)
        {
            // Check if we can start a new burst
            if (Time.time >= burstEndTime + burstCooldown)
            {
                StartBurst();
            }
        }
        else
        {
            // Currently bursting
            if (Time.time >= burstStartTime + burstDuration)
            {
                EndBurst();
            }
            else if (IsReadyToShoot())
            {
                PerformShoot();
            }
        }
    }

    private void StartBurst()
    {
        isBursting = true;
        burstStartTime = Time.time;
        canShoot = true;
    }

    private void EndBurst()
    {
        isBursting = false;
        burstEndTime = Time.time;
        canShoot = false;
    }

    protected override float GetInaccuracyAngle()
    {
        return inaccuracyAngle;
    }

    protected override void OnShootEffects()
    {
        // Add muzzle flash, shell ejection, etc.
        Debug.Log("Machine gun firing!");
    }

    public void ConfigureMachineGun(float _fireRate, float _inaccuracyAngle, float _burstDuration = 3f)
    {
        fireRate = _fireRate;
        inaccuracyAngle = _inaccuracyAngle;
        burstDuration = _burstDuration;
    }

    // Backwards compatibility method for existing GameManager code
    public void SetMachineGunEnenmy(float _fireRate, float _inaccuracyAngle)
    {
        ConfigureMachineGun(_fireRate, _inaccuracyAngle, burstDuration);
    }
}