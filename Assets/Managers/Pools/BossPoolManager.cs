//Boss
public class BossPoolManager : BasePoolManager<BossAttackPrefabType>
{
    private void Start()
    {
        foreach (ObjectPool<BossAttackPrefabType> pool in pools)
        {
            AddPool(pool.poolType, pool);
        }
    }
}