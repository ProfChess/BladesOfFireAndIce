using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BaseEnemy))]
public class RangedAttack : EnemyDamage, IEnemyAttackBehaviour
{
    [SerializeField] private GameObject ProjectilePrefab;
    public void Attack(float Damage, float Range, int Cooldown, float Duration, Transform playerTransform)
    {
        //Get Player Direction
        Vector2 PlayerDirection = GetPlayerDirection(playerTransform);

        //Spawn Arrow
        GameObject Arrow = PoolManager.Instance.getObjectFromPool(PoolType.ArrowProjectile);
        Arrow.transform.position = gameObject.transform.position;
        
        //Start Arrow Movement
        Arrow.GetComponent<EnemyProjectile>().ShootProjectile(Damage, Duration, PlayerDirection);
    }
    
}
