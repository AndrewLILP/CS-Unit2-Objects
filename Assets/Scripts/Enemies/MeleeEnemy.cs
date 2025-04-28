using UnityEngine;

public class MeleeEnemy : Enemy // : MonoBehaviour via Enemy and PlayableObject
{
    [SerializeField] private float attackRange = 1f; // The range within which the enemy can attack
    [SerializeField] private float attackTime = 1f; // The time it takes to perform an attack

    private float timer;
    private float setSpeed;

    protected override void Start()
    {
        base.Start();
        health = new Health(1, 0, 1);
    }

    protected override void Update()
    {
        base.Update();
        if (target == null)
        {
            return;
        }

        if (Vector2.Distance(transform.position, target.position) < attackRange)
        {
            speed = 0;
            Attack(attackTime);
        }
        else
        {
            speed = setSpeed;
        }
    }

    public override void Attack(float interval)
    {
        if (timer <= interval)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0f;
            target.GetComponent<IDamagable>().GetDamage(weapon.GetDamage());
            Debug.Log(weapon.GetDamage());
        }
    }
    public override void GetDamage(float damage)
    {
        base.GetDamage(damage);
    }

    public void SetMeleeEnemy(float _attackRange, float _attackTime)
    {
        attackRange = _attackRange;
        attackTime = _attackTime;

    }
}
