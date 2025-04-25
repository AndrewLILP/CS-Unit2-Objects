using UnityEngine;

public class Player //: MonoBehaviour
{
    private string nickName;
    private float projectileSpeed;
    public Health health = new Health(100, 0.5f, 100f); // this is public so we can decrease health if it gets hit in gameplay

    public void Move(Vector3 direction)
    {
        // Move the player in the specified direction
        // This method would be called in the Update method to move the player based on input
        // For example, you could use Input.GetAxis("Horizontal") and Input.GetAxis("Vertical") to get the input direction
        // and then call this method with that direction.

        Debug.Log("Moving player in direction: ");
    }

    public void Shoot(Vector3 direction, float projectileSpeed)
    {
        // Shoot a projectile in the specified direction
        // This method would be called when the player presses the shoot button
        // For example, you could use Input.GetButtonDown("Fire1") to check if the player is shooting
        // and then call this method with the direction and speed of the projectile.
        Debug.Log("Shooting projectile in direction: " + direction + " with speed: " + projectileSpeed);
    }

    public void Die()
    {
        // dont destroy the player, just set them to inactive
        Debug.Log("Player has died");
    }

}
