using UnityEngine;

public class ExploderEnemy : Enemy // : MonoBehaviour via Enemy and PlayableObject
{
    public float exploderRange;

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }
}
