using UnityEngine;

public class ShooterEnemy : Enemy //: MonoBehaviour via Enemy and PlayableObject
{
    private float shootingRate;

    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }

}
