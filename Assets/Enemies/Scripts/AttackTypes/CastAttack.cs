
using UnityEngine;

public class CastAttack : EnemyDamage, IEnemyAttackBehaviour
{
    [SerializeField] private GameObject prefab;
    public void Attack(float Damage, float Range, int Cooldown, float Offset, Transform playerTransform)
    {
        //Middle Fireball Direction
        Vector2 MiddleFireballDir = GetPlayerDirection(playerTransform);
        Vector2 AboveFireballDir = RotateVector(MiddleFireballDir, 30);
        Vector2 BelowFireballDir = RotateVector(MiddleFireballDir, -30);
        //Calculate Rotation
        GameObject MiddleFireball = PoolManager.Instance.getObjectFromPool(PoolType.FireProjectile);
        MiddleFireball.transform.position = transform.position;
        GameObject AboveFireball = PoolManager.Instance.getObjectFromPool(PoolType.FireProjectile);
        AboveFireball.transform.position = transform.position;
        GameObject BelowFireball = PoolManager.Instance.getObjectFromPool(PoolType.FireProjectile);
        BelowFireball.transform.position = transform.position;

        MiddleFireball.GetComponent<EnemyProjectile>().ShootProjectile(Damage, Offset, MiddleFireballDir);
        AboveFireball.GetComponent<EnemyProjectile>().ShootProjectile(Damage, Offset, AboveFireballDir);
        BelowFireball.GetComponent<EnemyProjectile>().ShootProjectile(Damage, Offset, BelowFireballDir);

    }
    private Vector2 RotateVector(Vector2 v, float d)
    {
        float rad = d * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos);
    }

}
