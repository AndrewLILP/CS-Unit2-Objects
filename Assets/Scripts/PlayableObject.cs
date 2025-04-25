using UnityEngine;

/// <summary>
/// think of abstract class as a concept - playableObject cannot be put on an onbject in the inspector but player can be. 
/// anything that exists at runtime in the game
/// we have 2 playable objects in the game - player and enemy
/// player and enemy derive from this class - this class is a monoBehaviour - this makes player and enemy a monoBehaviour
/// players uses shoot, enemy uses attack
/// virtual keyword allows inherited classes to override  
/// the class with monoBehaviour is the base class for all scripts in Unity - head honcho
/// abstract class - methods can be virtual or abstract
/// </summary>

public abstract class PlayableObject : MonoBehaviour 
{
    public Health health = new Health(); // this is public so we can decrease health if it gets hit in gameplay

    public Weapon weapon; // this is the weapon that the player or enemy will use

    public abstract void Move(Vector2 direction, Vector2 target); // virtual keyword allows this method to be overridden in derived classes
    //{
        // Debug.Log("Moving playable object"); - debugs removed when PO became an abstract class
    //}

    public abstract void Shoot();

    public abstract void Attack(float interval);

    public abstract void Die();

    public abstract void GetDamage(float damage);
}

// polymorphism - this is a base class that can be used to create different types of playable objects -
// eg, Move() is common but can have different parameters