using UnityEngine;

public class MeleeEnemy : Enemy // : MonoBehaviour via Enemy and PlayableObject
{
    [SerializeField] private float attackRange = 1f; // The range within which the enemy can attack
    [SerializeField] private float attackTime = 1f; // The time it takes to perform an attack

    // ==================== NEW: ENHANCED MOVEMENT VARIABLES ====================
    [Header("Enhanced Movement")]
    [SerializeField] private float zigzagIntensity = 2f; // How much the enemy zigzags
    [SerializeField] private float zigzagSpeed = 3f; // How fast the zigzag pattern changes
    [SerializeField] private float speedBurstMultiplier = 2f; // Speed boost when close
    [SerializeField] private float speedBurstRange = 3f; // Range to trigger speed burst

    private float zigzagTimer = 0f;
    private Vector2 zigzagOffset;
    private bool isSpeedBoosted = false;
    // ============================================================================

    private float timer;
    private float setSpeed;

    protected override void Start()
    {
        base.Start();
        health = new Health(1, 0, 1);
        setSpeed = speed; // Store original speed
    }

    protected override void Update()
    {
        base.Update();
        if (target == null)
        {
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        if (distanceToTarget < attackRange)
        {
            speed = 0;
            Attack(attackTime);
        }
        else
        {
            // ==================== NEW: ENHANCED MOVEMENT LOGIC ====================
            HandleEnhancedMovement(distanceToTarget);
            // ========================================================================
        }
    }

    // ==================== NEW: ENHANCED MOVEMENT METHOD ====================
    private void HandleEnhancedMovement(float distanceToTarget)
    {
        // Check if we should use speed burst
        if (distanceToTarget <= speedBurstRange && !isSpeedBoosted)
        {
            speed = setSpeed * speedBurstMultiplier;
            isSpeedBoosted = true;
            Debug.Log("Melee enemy speed boost activated!");
        }
        else if (distanceToTarget > speedBurstRange && isSpeedBoosted)
        {
            speed = setSpeed;
            isSpeedBoosted = false;
        }
        else if (distanceToTarget > speedBurstRange)
        {
            speed = setSpeed;
        }

        // Update zigzag pattern
        zigzagTimer += Time.deltaTime * zigzagSpeed;
        zigzagOffset = new Vector2(
            Mathf.Sin(zigzagTimer) * zigzagIntensity,
            Mathf.Cos(zigzagTimer * 0.7f) * zigzagIntensity * 0.5f
        );
    }

    // MODIFIED: Enhanced Move method with zigzag
    public override void Move(Vector2 direction)
    {
        // Apply zigzag offset to movement direction
        Vector2 enhancedDirection = direction + zigzagOffset;

        enhancedDirection.x -= transform.position.x;
        enhancedDirection.y -= transform.position.y;

        float angle = Mathf.Atan2(enhancedDirection.y, enhancedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), Time.deltaTime * 5f);
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    // ========================================================================

    public override void Attack(float interval)
    {
        if (timer <= interval)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            target.GetComponent<IDamagable>().GetDamage(weapon.GetDamage());
            Debug.Log($"Melee attack for {weapon.GetDamage()} damage!");
        }
    }

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }

    public void SetMeleeEnemy(float _attackRange, float _attackTime)
    {
        attackRange = _attackRange;
        attackTime = _attackTime;
    }

    // ==================== NEW: CONFIGURATION METHOD ====================
    public void ConfigureMeleeEnemy(float _attackRange, float _attackTime, float _zigzagIntensity = 2f, float _speedBurstMultiplier = 2f)
    {
        attackRange = _attackRange;
        attackTime = _attackTime;
        zigzagIntensity = _zigzagIntensity;
        speedBurstMultiplier = _speedBurstMultiplier;
    }
    // ====================================================================
}