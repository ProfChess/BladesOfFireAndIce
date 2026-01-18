//PlayerEffects
public class PlayerEffectPoolManager : BasePoolManager<PlayerEffectObjectType>
{
    private void Start()
    {
        foreach (ObjectPool<PlayerEffectObjectType> pool in pools)
        {
            AddPool(pool.poolType, pool);
        }
    }
}