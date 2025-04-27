using UnityEngine;

public class MeleeEnemy : Enemy // : MonoBehaviour via Enemy and PlayableObject
{
    private float attackRange = 1f; // The range within which the enemy can attack
    private float attackTime = 1f; // The time it takes to perform an attack
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }

    public void SetMeleeEnemy(float _attackRange, float _attackTime)
    {
        attackRange = _attackRange;
        attackTime = _attackTime;

    }
}
