using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectPoolManager : MonoBehaviour
{
    public static PlayerEffectPoolManager Instance;
    [SerializeField] private List<PlayerEffectObjectPool> pools = new List<PlayerEffectObjectPool>();
    public Dictionary<PlayerEffectObjectType, PlayerEffectObjectPool> poolsDict = new Dictionary<PlayerEffectObjectType, PlayerEffectObjectPool>();

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
    public GameObject getObjectFromPool(PlayerEffectObjectType poolType)
    {
        if (poolsDict.ContainsKey(poolType))
        {
            return poolsDict[poolType].getObject();
        }
        Debug.Log("Pool is not found");
        return null;
    }

    //Returns object to correct pool based on number given
    public void ReturnObjectToPool(PlayerEffectObjectType poolType, GameObject self)
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
