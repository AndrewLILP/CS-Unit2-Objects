using UnityEngine;

public class Weapon //: NOT MonoBehaviour
{
    private string name; // can use name because it is not a MonoBehaviour - ie it is in the Weapon class
    private float damage = 1f; // damage is a float so we can have different values for different weapons - default now is 1

    // Constructor for the Weapon class - using _name and _damage as parameters - these can be changed in player

    public Weapon(string _name, float _damage)
    {
        // Constructor for the Weapon class
        // This constructor can be used to initialize the weapon with default values
        // or to create a new weapon with specific values.
        // For example, you could create a new weapon with a specific name and damage value.

        name = _name;
        damage = _damage;

    }

    public Weapon() // default constructor
    {
        // Default constructor for the Weapon class
        // This constructor can be used to create a new weapon with default values.
        // For example, you could create a new weapon with a default name and damage value.
    }

    public void Shoot()
    {
        // Shoot the weapon
        // This method would be called when the player presses the shoot button
        // For example, you could use Input.GetButtonDown("Fire1") to check if the player is shooting
        // and then call this method to shoot the weapon.
        Debug.Log("Shooting weapon: " + name + " with damage: " + damage);
    }


}
