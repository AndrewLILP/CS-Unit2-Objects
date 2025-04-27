using UnityEngine;

public abstract class PlayableObject : MonoBehaviour , IDamagable
{
    public Health health = new Health(); // this is public so we can decrease health if it gets hit in gameplay

    public Weapon weapon; // this is the weapon that the player or enemy will use

    public abstract void Move(Vector2 direction, Vector2 target); // virtual keyword allows this method to be overridden in derived classes
                                                                  //{
                                                                  // Debug.Log("Moving playable object"); - debugs removed when PO became an abstract class
                                                                  //}

    // polymorphism
    public virtual void Move(Vector2 direction) { } // scope { } as it is in abstract class

    // all playableObjects MUST use above Move and CAN optionally use Moves below 
    public virtual void Move(float speed) { }
    public abstract void Shoot();

    public abstract void Attack(float interval);

    public abstract void Die();

    public virtual void GetDamage(float damage)  // abstract classes can have virtual methods 
    {
        health.DeductHealth(damage); // check health = 0 when damage is done - add to Health.cs
        if (health.GetHealth() <= 0)
        {
            Die();
        }

    }
}
// When to use Abstract class - share logic across subclasses - methods must be used 


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



// polymorphism - this is a base class that can be used to create different types of playable objects -
// eg, Move() is common but can have different parameters