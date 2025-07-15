using UnityEngine;

public class Weapon //: NOT MonoBehaviour
{
    private string name; // can use name because it is not a MonoBehaviour - ie it is in the Weapon class
    private float damage = 1f; // damage is a float so we can have different values for different weapons - default now is 1
    private float bulletSpeed;

    // Constructor for the Weapon class - using _name and _damage as parameters - these can be changed in player

    public Weapon(string _name, float _damage, float _bulletSpeed)
    {
        // Constructor for the Weapon class
        // This constructor can be used to initialize the weapon with default values
        // or to create a new weapon with specific values.
        // For example, you could create a new weapon with a specific name and damage value.

        name = _name;
        damage = _damage;
        bulletSpeed = _bulletSpeed;

    }

    public Weapon() // default constructor
    {
        // Default constructor for the Weapon class
        // This constructor can be used to create a new weapon with default values.
        // For example, you could create a new weapon with a default name and damage value.
    }

    public void Shoot(Bullet _bullet, Transform _firePoint, string _targetTag, float _timeToLive = 5f)
    {
        // Shoot the weapon
        // This method would be called when the player presses the shoot button
        // For example, you could use Input.GetButtonDown("Fire1") to check if the player is shooting
        // and then call this method to shoot the weapon.
        Debug.Log("Shooting weapon: " + name + " with damage: " + damage);

        // ==================== NEW: MUZZLE FLASH EFFECT ====================
        // Play muzzle flash effect at fire point
        if (_firePoint != null)
        {
            // Calculate rotation for muzzle flash (align with weapon direction)
            float rotation = _firePoint.rotation.eulerAngles.z;
            EffectsManager.PlayMuzzleFlash(_firePoint.position, rotation);
        }
        // ===================================================================

        Bullet tempBullet = GameObject.Instantiate(_bullet, _firePoint.position, _firePoint.rotation); // instantiate the bullet prefab
        tempBullet.SetBullet(damage, _targetTag, bulletSpeed); // set the bullet's damage and target tag

        GameObject.Destroy(tempBullet.gameObject, _timeToLive); // destroy the bullet after a certain time
    }

    public float GetDamage()
    {
        // Return the damage value of the weapon
        return damage;
    }


}