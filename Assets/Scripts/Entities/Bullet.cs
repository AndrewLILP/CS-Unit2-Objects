using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;

    private void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D other) // collision changed to other
    {
        // OnCollisionEnter2D(Collision2D collision); could be used - notice Collider2D and Collision2D
        Debug.Log(other.gameObject.name);
        IDamagable damageable = other.GetComponent<IDamagable>(); // go up to get other
        Damage(damageable);
    }

    void Damage(IDamagable damageable)
    {
        if (damageable != null)
        {
            damageable.GetDamage(damage);
            Debug.Log("Damaged something");
            // Add score
            // Destroy gameobject

        } // else {do nothing / something when null


    }

    private void Move()
    {
        // no rigid body as bullet works like a lazer in this game

        transform.Translate(Vector2.right * speed * Time.deltaTime);

    }
}
