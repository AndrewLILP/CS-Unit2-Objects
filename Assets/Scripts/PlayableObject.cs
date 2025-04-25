using UnityEngine;

/// <summary>
/// anything that exists at runtime in the game
/// we have 2 playable objects in the game - player and enemy
/// player and enemy derive from this class - this class is a monoBehaviour - this makes player and enemy a monoBehaviour
/// players uses shoot, enemy uses attack
/// virtual keyword allows inherited classes to override  
/// </summary>

public class PlayableObject : MonoBehaviour 
{
    public Health health = new Health(); // this is public so we can decrease health if it gets hit in gameplay

    public Weapon weapon; // this is the weapon that the player or enemy will use

    public virtual void Move() // virtual keyword allows this method to be overridden in derived classes
    {
        Debug.Log("Moving playable object");
    }

    public virtual void Shoot(Vector3 direction, float speed)
    {
        Debug.Log("Shooting bullet using: " + direction + speed);
    }

    public virtual void Attack(float interval)
    {
        Debug.Log("Attacking at interval" + interval);
    }

    public virtual void Die()
    {
        // dont destroy the object, just set them to inactive
        Debug.Log("Playable object has died" + gameObject.name);
    }
}

// polymorphism - this is a base class that can be used to create different types of playable objects -
// eg, Move() is common but can have different parameters