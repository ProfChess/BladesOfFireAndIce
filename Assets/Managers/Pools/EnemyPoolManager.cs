//Enemies
public class EnemyPoolManager : BasePoolManager<EnemyType>
{
    private void Start()
    {
        foreach (ObjectPool<EnemyType> pool in pools)
        {
            AddPool(pool.poolType, pool);
        }
    }
}


