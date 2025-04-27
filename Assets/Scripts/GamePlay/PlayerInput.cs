using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Player player;
    private float horizontal, vertical;
    private Vector2 lookTarget;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        lookTarget = Input.mousePosition;

        if (Input.GetMouseButtonDown(0)) // GetMouseButton(0) is rapid fire - 0 is the left mouse button
        {
            // Call the Shoot method on the player when the left mouse button is pressed
            // This will shoot the weapon and instantiate a bullet
            // The player class has a Shoot method that handles this logic
            // The player class also has a weapon property that is used to shoot the bullet
            // The player class also has a firePoint property that is used to instantiate the bullet
            
                player.Shoot();
        }
    }

    private void FixedUpdate()
    {
        
        player.Move(new Vector2(horizontal, vertical), lookTarget);
        
    }
}
