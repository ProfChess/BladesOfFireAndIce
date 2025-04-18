using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class ChargeAttack : EnemyDamage, IEnemyAttackBehaviour
{
    public void Attack(float Damage, float Range, int Cooldown, float Speed, Transform playerTransform)
    {

    }
}
