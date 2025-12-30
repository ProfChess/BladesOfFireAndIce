using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum PlayerEffectObjectType { FlameExplosion, IceExplosion, IceProj}

public class PlayerEffectObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab; //Object kept in the pool
    [SerializeField] private int baseSize = 3;  //Number of objects to pre-instantiate

    private List<GameObject> pool; //Pool of objects

    public PlayerEffectObjectType poolType;

    private void Awake()
    {
        pool = new List<GameObject>();

        //Instantiate/disable base number of objects 
        for (int i = 0; i < baseSize; i++)
        {
            GameObject obj = Instantiate(prefab, this.transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject getObject()
    {
        //Check if there is an inactive object in the pool
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true); //Activate and return the object
                return obj;
            }
        }

        //If no object is available, create a new one
        GameObject newObj = Instantiate(prefab, this.transform);
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }

    public void returnObject(GameObject obj)
    {
        obj.SetActive(false); //Disable object
        obj.transform.SetParent(this.transform); //Return to pool hierarchy
        obj.transform.position = this.transform.position;
    }

    public List<GameObject> getList()
    {
        return pool;
    }

}
