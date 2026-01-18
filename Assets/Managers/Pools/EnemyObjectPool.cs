public enum EnemyType
{
    Slime,
    Ranged,
    ArrowProjectile,
    Charger,
    Caster,
    FireProjectile,
    EliteEnemy1,
    EliteEnemy2,
    Invalid
}
public class EnemyObjectPool : ObjectPool<EnemyType> { }