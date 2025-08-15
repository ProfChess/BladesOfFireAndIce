using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    [SerializeField] private List<ObjectPool> pools = new List<ObjectPool>();
    public Dictionary<EnemyType, ObjectPool> poolsDict = new Dictionary<EnemyType, ObjectPool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            poolsDict.Add(pools[i].poolType, pools[i]);
        }
    }
    public GameObject getObjectFromPool(EnemyType poolType) 
    {
        if (poolsDict.ContainsKey(poolType))
        {
            return poolsDict[poolType].getObject();
        }
        Debug.Log("Pool is not found");
        return null;
    }

    //Returns object to correct pool based on number given
    public void ReturnObjectToPool(EnemyType poolType, GameObject self)
    {
        if (poolsDict.ContainsKey(poolType))
        {
            poolsDict[poolType].returnObject(self);
            return;
        }
        Debug.Log("Pool is not found");
        return;
    }
}
