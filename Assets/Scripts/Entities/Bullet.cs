using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private string targetTag;

    public void SetBullet(float _damage, string _targetTag, float _speed = 10f)
    {
        damage = _damage;
        targetTag = _targetTag;
        speed = _speed;
    }  

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other) // collision changed to other
    {

        // OnCollisionEnter2D(Collision2D collision); could be used - notice Collider2D and Collision2D
        Debug.Log(other.gameObject.name);
        
        if (!other.gameObject.CompareTag(targetTag)) // if not the tag we asked for, do nothing - !other.gameObject - casts up to game object
        {
            return;
        }

        //if (other.gameObject.CompareTag(targetTag)) - alternate {} not needed because it is 1 line
          //  return; 
        

        IDamagable damageable = other.GetComponent<IDamagable>(); // go up to get other
        Damage(damageable);

    }

    void Damage(IDamagable damageable)
    {
        if (damageable != null)
        {
            damageable.GetDamage(damage);
            Debug.Log("Damaged something");


            // Add score - will use game manager to do this
            GameManager.GetInstance().scoreManager.IncrementScore();
            
            Destroy(gameObject); // destroy the bullet

        } // else {do nothing / something when null


    }

    private void Move()
    {
        // no rigid body as bullet works like a lazer in this game - it has one but probably not needed

        transform.Translate(Vector2.up * speed * Time.deltaTime);

    }
}
