using UnityEngine;

public class Player : PlayableObject //: MonoBehaviour
{
    private string nickName;
    private float projectileSpeed;

    private void Start()
    {
        health = new Health(100, 0.5f, 100f); // this is public so we can decrease health if it gets hit in gameplay
    }
    

    public Player()
    {         // Constructor for the Player class
        // This constructor can be used to initialize the player with default values
        // or to create a new player with specific values.
        // For example, you could create a new player with a specific name and health value.
        
    }

    public override void Move()
    {
        // Move the player in the specified direction
        // This method would be called in the Update method to move the player based on input
        // For example, you could use Input.GetAxis("Horizontal") and Input.GetAxis("Vertical") to get the input direction
        // and then call this method with that direction.

        base.Move();
        // Call the base class Move method to handle the movement logic
        // This allows you to reuse the base class functionality while adding player-specific behavior
        Debug.Log("Moving player in direction: ");
    }

    public override void Die()
    {
        // dont destroy the player, just set them to inactive
        Debug.Log("Player has died");
    }

}
