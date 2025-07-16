// NukePickup.cs - SIMPLE VERSION
// Replace your entire NukePickup.cs with this simple version

using UnityEngine;

public class NukePickup : Pickup
{
    public override void OnPicked()
    {
        // Add nuke to manager
        NukeManager nukeManager = NukeManager.GetInstance();
        if (nukeManager != null)
        {
            nukeManager.AddNuke();
        }

        // Simple pickup effect
        CameraShake.ShakeLight();

        base.OnPicked(); // This destroys the pickup GameObject
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnPicked();
        }
    }
}