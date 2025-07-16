// PlayerInput.cs - SIMPLE MODIFICATIONS
// In your existing PlayerInput.cs, ADD these sections marked NEW

using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // ========== EXISTING CODE - DON'T CHANGE ==========
    private Player player;
    private float horizontal, vertical;
    private Vector2 lookTarget;

    void Start()
    {
        player = GetComponent<Player>();
    }

    // ========== MODIFY UPDATE METHOD ==========
    void Update()
    {
        // EXISTING - DON'T CHANGE
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        lookTarget = Input.mousePosition;

        // EXISTING - DON'T CHANGE
        if (Input.GetMouseButtonDown(0))
        {
            player.Shoot();
        }

        // ========== NEW: Add right-click for nuke ==========
        if (Input.GetMouseButtonDown(1))
        {
            NukeManager nukeManager = NukeManager.GetInstance();
            if (nukeManager != null && nukeManager.CanUseNuke())
            {
                nukeManager.UseNuke();
            }
            else
            {
                Debug.Log("No nukes available!");
            }
        }

        // DEBUG: Add nuke for testing (remove in production)
        if (Input.GetKeyDown(KeyCode.N))
        {
            NukeManager.GetInstance()?.AddNuke();
        }
        // =================================================
    }

    // ========== EXISTING CODE - DON'T CHANGE ==========
    private void FixedUpdate()
    {
        player.Move(new Vector2(horizontal, vertical), lookTarget);
    }
}