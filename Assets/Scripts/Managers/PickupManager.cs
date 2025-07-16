// PickupManager.cs - MODIFICATIONS TO EXISTING FILE
// Find your existing PickupManager.cs and add the marked NEW sections

using System.Security;
using UnityEngine;
using System.Collections.Generic;
using System.Collections; // NEW: Add this import

public class PickupManager : MonoBehaviour
{
    // ========== EXISTING CODE - DON'T CHANGE ==========
    [SerializeField] private PickupSpawn[] pickups;

    [Range(0, 1)]
    [SerializeField] private float pickupProbability;

    // ========== NEW: Add pickup lifetime management fields ==========
    [Header("Pickup Lifetime Management")] // NEW SECTION
    [SerializeField] private float pickupLifetime = 150f; // NEW
    [SerializeField] private float warningTime = 3f; // NEW
    [SerializeField] private bool enablePickupTimeout = true; // NEW
    // ===============================================================

    // ========== EXISTING CODE - DON'T CHANGE ==========
    List<Pickup> pickupPool = new List<Pickup>();
    Pickup chosenPickup;

    // ========== NEW: Add pickup tracking lists ==========
    List<PickupInstance> activePickups = new List<PickupInstance>(); // NEW
    // ==================================================

    // ========== EXISTING CODE - MODIFY START METHOD ==========
    void Start()
    {
        // EXISTING - DON'T CHANGE
        foreach (PickupSpawn spawn in pickups)
        {
            for (int i = 0; i < spawn.spawnWeight; i++)
            {
                pickupPool.Add(spawn.pickup);
            }
        }

        // ========== NEW: Start pickup lifetime management ==========
        // Start cleanup coroutine
        if (enablePickupTimeout)
        {
            StartCoroutine(ManagePickupLifetimes());
        }
        // =========================================================
    }

    // ========== EXISTING CODE - MODIFY SpawnPickup METHOD ==========
    public void SpawnPickup(Vector2 position)
    {
        // EXISTING - DON'T CHANGE
        if (pickupPool.Count == 0)
            return;

        if (Random.Range(0.0f, 1.0f) < pickupProbability)
        {
            // EXISTING - DON'T CHANGE
            chosenPickup = pickupPool[Random.Range(0, pickupPool.Count)];

            // ========== MODIFIED: Change Instantiate call ==========
            GameObject spawnedPickup = Instantiate(chosenPickup.gameObject, position, Quaternion.identity);

            // ========== NEW: Track the pickup for lifetime management ==========
            // Track the pickup for lifetime management
            if (enablePickupTimeout)
            {
                PickupInstance instance = new PickupInstance
                {
                    pickup = spawnedPickup.GetComponent<Pickup>(),
                    spawnTime = Time.time,
                    hasWarned = false
                };
                activePickups.Add(instance);
            }
            // ===============================================================
        }
    }

    // ========== NEW: Add these new methods ==========
    private IEnumerator ManagePickupLifetimes()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            for (int i = activePickups.Count - 1; i >= 0; i--)
            {
                if (activePickups[i].pickup == null)
                {
                    // Pickup was collected or destroyed
                    activePickups.RemoveAt(i);
                    continue;
                }

                float age = Time.time - activePickups[i].spawnTime;

                // Warning phase
                if (!activePickups[i].hasWarned && age >= pickupLifetime - warningTime)
                {
                    StartCoroutine(WarnPickupExpiring(activePickups[i]));
                    activePickups[i].hasWarned = true;
                }

                // Expiration
                if (age >= pickupLifetime)
                {
                    ExpirePickup(activePickups[i]);
                    activePickups.RemoveAt(i);
                }
            }
        }
    }

    private IEnumerator WarnPickupExpiring(PickupInstance instance)
    {
        if (instance.pickup == null) yield break;

        SpriteRenderer renderer = instance.pickup.GetComponent<SpriteRenderer>();
        if (renderer == null) yield break;

        Color originalColor = renderer.color;

        // Blink red to warn about expiration
        for (int i = 0; i < 6; i++)
        {
            if (instance.pickup == null) break;

            renderer.color = Color.red;
            yield return new WaitForSeconds(0.2f);

            if (instance.pickup == null) break;

            renderer.color = originalColor;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void ExpirePickup(PickupInstance instance)
    {
        if (instance.pickup != null)
        {
            // Small effect when pickup expires
            EffectsManager.PlayBulletImpact(instance.pickup.transform.position);
            Destroy(instance.pickup.gameObject);
            Debug.Log("Pickup expired and was removed");
        }
    }

    public void ClearAllPickups()
    {
        foreach (var instance in activePickups)
        {
            if (instance.pickup != null)
            {
                Destroy(instance.pickup.gameObject);
            }
        }
        activePickups.Clear();
    }

    public int GetActivePickupCount()
    {
        return activePickups.Count;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    // =============================================

    // ========== EXISTING CODE - DON'T CHANGE ==========
    void Update()
    {

    }
}

// ========== EXISTING CODE - DON'T CHANGE ==========
[System.Serializable]
public struct PickupSpawn
{
    public Pickup pickup;
    public int spawnWeight;
}

// ========== NEW: Add this new class at the bottom of the file ==========
[System.Serializable]
public class PickupInstance
{
    public Pickup pickup;
    public float spawnTime;
    public bool hasWarned;
}
// ====================================================================