using UnityEngine;

public class Enemy : PlayableObject //: not MonoBehaviour - player and enemy will come from character class (abstract class)
{
    
    private string enemyName;
    

    private EnemyType enemyType; // enum for different types of enemies
    [SerializeField] protected Transform target; // protected is also not in Inspector
    // the target that the enemy will move towards (eg the player)
    [SerializeField] protected float speed;


    // lots of enemies can be created with different values for Health
    // weappons can be created with different values for Health damage

    protected virtual void Start()
    {
        target = GameObject.FindWithTag("Player").transform;

        health = new Health(10, 0.1f, 10f); // this is public so we can decrease health if it gets hit in gameplay
    }

    protected virtual void Update()
    {
        if (target != null)
        {
            Move(target.position);
        }
        else
        {
            // eg player uses a smoke screen
            Move(speed);
        }
    }

    public void SetEnemyType(EnemyType _enemyType)
    {
        enemyType = _enemyType;
        // this.enemyType = _enemyType; is the old way to do it

        // we will use this to set the enemy type in the inspector and/or spwaning enemies
    }

    public override void Move(Vector2 direction)
    {
        direction.x -= transform.position.x;
        direction.y -= transform.position.y;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);
        transform.Translate(Vector2.right * speed * Time.deltaTime);

    }

    public override void Move(float speed)
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    // the Move below is a MUST have - it is abstract in Pl-Ob
    public override void Move(Vector2 direction, Vector2 target)
    {
        // empty move mostly for player - enemy would target mouse position - could be interesting feature
        // will follow target
        // Move the enemy in the specified direction - towards the target (eg the player)
        // This method would be called in the Update method to move the enemy based on AI or player input
        // For example, you could use a NavMeshAgent to move the enemy towards the player
        Debug.Log("Moving towards: ");
    }

    public override void Die()
    {
        Debug.Log("Enemy died: ");
        GameManager.GetInstance().NotifyDeath(this);
        Destroy(gameObject);
        //base.Die(); base doesnt work any more
        // more stuff can be added to the stuff
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }

    public override void GetDamage(float damage)
    {
        //throw new System.NotImplementedException();
        health.DeductHealth(damage); // check health = 0 when damage is done - add to Health.cs
        if (health.GetHealth() <= 0)
        {
            Die();
        }
    }

    public override void Attack(float interval)
    {
        Debug.Log("Enemy attacking with interval: ");
    }

}

// This class is used to define the enemy in the game.
// It can be used to create different enemy behaviors and attributes based on their type.
// For example, a Melee enemy might have a different attack pattern than a Shooter enemy.
// This class can be used in conjunction with the EnemyType enum to create a more complex enemy system.
// private variables
// enemy has health because it is a playable object - health is a variable in playableObject
