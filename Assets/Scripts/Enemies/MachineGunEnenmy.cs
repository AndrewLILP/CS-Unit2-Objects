using UnityEngine;

/// <summary>
/// MachineGunEnemy class inherits from ShooterEnemy.
/// it needs to cool down after shooting.
/// </summary>

public class MachineGunEnenmy : ShooterEnemy // : MonoBehaviour via ShooterEnemy Enemyand PlayableObject
{
    public float shootingTime;
    public float shootingCoolDownTime;
}
