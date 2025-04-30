using UnityEngine;

public class HealthPickup : Pickup, IDamagable
{
    [SerializeField] private float healthToAdd = 1f;
    public override void OnPicked()
    {
        base.OnPicked();
        // increase health
        var player = GameManager.GetInstance().GetPlayer();
        player.health.AddHealth(healthToAdd);
        GameManager.GetInstance().uiManager.UpdateHealth(player.health.GetHealth());

    }

    public void GetDamage(float damage)
    {
        
        OnPicked();
    }

  

}
