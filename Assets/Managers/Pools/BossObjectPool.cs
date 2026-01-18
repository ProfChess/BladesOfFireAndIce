public enum BossAttackPrefabType
{
    LingeringFlames,
    SmallExplosions,
    CircleFlames,
    FlameWaves,
    Invalid
}
public class BossObjectPool : ObjectPool<BossAttackPrefabType> { }