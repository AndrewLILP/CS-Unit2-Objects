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
    }

    public void GetDamage(float damage)
    {
        
        OnPicked();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPicked();
        }
    }

}
