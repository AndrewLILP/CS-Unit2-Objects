using UnityEngine;

public class MeleeEnemy : Enemy // : MonoBehaviour via Enemy and PlayableObject
{
    public float attackRange = 1f; // The range within which the enemy can attack

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }
}
