using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    public List<ObjectPool> pools;

    private void Awake()
    {
        Instance = this;
    }
    public GameObject getObjectFromPool(PoolType poolType) 
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].poolType == poolType)
            {
                return pools[i].getObject();
            }
        }
        Debug.Log("Pool is not found");
        return null;

    }

    //Returns object to correct pool based on number given
    public void ReturnObjectToPool(PoolType poolName, GameObject self)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i].poolType == poolName)
            {
                pools[i].returnObject(self);
                return;
            }
        }
        Debug.Log("Pool is not found");
        return;
    }
}
