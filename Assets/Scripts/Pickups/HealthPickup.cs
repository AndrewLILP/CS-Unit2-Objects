using UnityEngine;

public class HealthPickup : Pickup, IDamagable
{
    public override void OnPicked()
    {
        base.OnPicked();
        // increase health
    }

    public void GetDamage(float damage)
    {
        // add health
    }

}
