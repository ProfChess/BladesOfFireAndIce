using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePoolManager<TKey> : MonoBehaviour where TKey : Enum
{
    public static BasePoolManager<TKey> Instance { get; private set; }

    protected Dictionary<TKey, ObjectPool<TKey>> poolDict = new();
    [SerializeField] protected List<ObjectPool<TKey>> pools = new();

    public void AddPool(TKey key, ObjectPool<TKey> pool) => poolDict[key] = pool;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject getObjectFromPool(TKey key)
    {
        if (poolDict.TryGetValue(key, out ObjectPool<TKey> pool))
        {
            return pool.getObject();
        }
        Debug.Log("Pool is not found");
        return null;
    }

    //Returns object to correct pool based on number given
    public void ReturnObjectToPool(TKey key, GameObject self)
    {
        if (poolDict.TryGetValue(key, out ObjectPool<TKey> pool))
        {
            pool.returnObject(self);
            return;
        }
        Debug.Log("Pool is not found");
        return;
    }

}
