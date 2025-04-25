using UnityEngine;
// Inheritance: Player inherits from PlayableObject

public class Player : PlayableObject //: MonoBehaviour
{
    [SerializeField] private string nickName;
    [SerializeField] private float Speed;
    [SerializeField] private Camera cam;

    private Rigidbody2D playerRB; // player must have a rigidbody2D - filled dynamically in Start() method - errors will occur if not

    private void Start()
    {
        health = new Health(100f, 0.5f, 100f); // this is public so we can decrease health if it gets hit in gameplay
        playerRB = GetComponent<Rigidbody2D>(); // get the rigidbody2D component attached to the player
    }
    
    public override void Move(Vector2 direction, Vector2 target)
    {
        playerRB.linearVelocity = direction * Speed * Time.deltaTime; // set the velocity of the player based on the direction and speed

        Vector3 playerScreenPos = cam.WorldToScreenPoint(transform.position); // get the screen position of the player
        target.x -= playerScreenPos.x; // subtract the screen position of the player from the target position
        target.y -= playerScreenPos.y; // subtract the screen position of the player from the target position

        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg; // get the angle between the player and the target position
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // rotate the player towards the target position

        //var playerScreenPos = cam.WorldToScreenPoint(transform.position); // get the screen position of the player but more expensive
        // var sets the variable to type Vector3 here - same as RHS

        // Move the player in the specified direction
        // This method would be called in the Update method to move the player based on input
        // For example, you could use Input.GetAxis("Horizontal") and Input.GetAxis("Vertical") to get the input direction
        // and then call this method with that direction.

        //base.Move();
        // Call the base class Move method to handle the movement logic
        // This allows you to reuse the base class functionality while adding player-specific behavior
        // Debug.Log("Moving player in direction: ");
    }

    public override void Die()
    {
        // dont destroy the player, just set them to inactive
        Debug.Log("Player has died");
    }

    public override void Shoot()
    {
        //throw new System.NotImplementedException(); - this works in a similar way to debug.log
        Debug.Log("Player Shooting a bullet ");
    }

    public override void Attack(float interval)
    {
        // attack is meant for the enemy and not player - but it must be implemented because it is an abstract method in the base class - leave empty for now
        // melee attack for enemy - could be removed from PlayableObject if player doesnt have an attack
        throw new System.NotImplementedException();
    }

    public override void GetDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

}
