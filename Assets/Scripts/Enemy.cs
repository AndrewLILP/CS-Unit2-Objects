using UnityEngine;

public class Enemy //: not MonoBehaviour - player and enemy will come from character class (abstract class)
{
    // This class is used to define the enemy in the game.
    // It can be used to create different enemy behaviors and attributes based on their type.
    // For example, a Melee enemy might have a different attack pattern than a Shooter enemy.
    // This class can be used in conjunction with the EnemyType enum to create a more complex enemy system.
    // private variables

    private string name;
    private float speed;

    private EnemyType enemyType; // enum for different types of enemies

    public Health health = new Health(10, 0.1f, 10);


    // lots of enemies can be created with different values for Health
    // weappons can be created with different values for Health damage

    public void SetEnemyType(EnemyType _enemyType)
    {
        enemyType = _enemyType;
        // this.enemyType = _enemyType; is the old way to do it

        // we will use this to set the enemy type in the inspector and/or spwaning enemies
    }

    public void Move(Vector3 direction)
    {
        // Move the enemy in the specified direction - towards the target (eg the player)
        // This method would be called in the Update method to move the enemy based on AI or player input
        // For example, you could use a NavMeshAgent to move the enemy towards the player
        Debug.Log("Moving enemy in direction: " + direction);
    }

    public void Die()
    {
        // dont destroy the enemy, just set them to inactive
        // this is so we can reuse the enemy object later
        Debug.Log("Enemy has died");
    }

}
