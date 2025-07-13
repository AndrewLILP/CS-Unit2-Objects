using UnityEngine;

/// <summary>
/// Exploding Enemy: approaches the enemy and explodes, performing heavy damage to the player
/// </summary>

public class ExploderEnemy : Enemy 
{
    public float exploderRange = 2f;
    public float explosionDamage = 5f;
    public float explosionRadius = 3f;

    protected override void Start()
    {
        base.Start();
        health = new Health(1, 0, 1); // Set initial health for the ExploderEnemy
    
    }

    protected override void Update()
    {
        base.Update();
        if (target == null)
        {
            return;
        }
        // Check if the enemy is within the explosion range of the target
        if (Vector2.Distance(transform.position, target.position) < exploderRange)
        {
            Explode();
        }
    }
    public override void GetDamage(float damage)
    {
        //base.GetDamage(damage);
        Explode();
    }

    private void Explode()
    {
        // Deal damage to the target
        if (target != null)
        {
            target.GetComponent<IDamagable>().GetDamage(explosionDamage);
            Debug.Log($"ExploderEnemy exploded, dealing {explosionDamage} damage to the target.");
        }
        
        // Maybe add explosion effect (can be a particle system or sound)
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (var hitCollider in hitColliders)
        {
            IDamagable damageable = hitCollider.GetComponent<IDamagable>();
            if (damageable != null && hitCollider.CompareTag("Player"))
            {
                damageable.GetDamage(explosionDamage);
            }
        }

        Debug.Log($"ExploderEnemy exploded with radius {explosionRadius}, dealing {explosionDamage} damage.");
        Die();
    }
}
