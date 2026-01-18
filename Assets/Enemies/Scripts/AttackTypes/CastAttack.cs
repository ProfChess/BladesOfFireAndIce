
using UnityEngine;

public class CastAttack : EnemyDamage, IEnemyAttackBehaviour
{
    [SerializeField] private GameObject prefab;
    public void Attack(float Range, int Cooldown, float Offset, Transform playerTransform)
    {
        //Middle Fireball Direction
        Vector2 MiddleFireballDir = GetPlayerDirection(playerTransform);
        Vector2 AboveFireballDir = RotateVector(MiddleFireballDir, 30);
        Vector2 BelowFireballDir = RotateVector(MiddleFireballDir, -30);
        //Calculate Rotation
        GameObject MiddleFireball = PM.getObjectFromPool(EnemyType.FireProjectile);
        MiddleFireball.transform.position = transform.position;
        GameObject AboveFireball = PM.getObjectFromPool(EnemyType.FireProjectile);
        AboveFireball.transform.position = transform.position;
        GameObject BelowFireball = PM.getObjectFromPool(EnemyType.FireProjectile);
        BelowFireball.transform.position = transform.position;

        MiddleFireball.GetComponent<EnemyProjectile>().ShootProjectile(Offset, MiddleFireballDir);
        AboveFireball.GetComponent<EnemyProjectile>().ShootProjectile(Offset, AboveFireballDir);
        BelowFireball.GetComponent<EnemyProjectile>().ShootProjectile(Offset, BelowFireballDir);

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
