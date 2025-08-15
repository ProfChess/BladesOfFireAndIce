using UnityEngine;

public enum ElementType { Fire, Ice, }
public abstract class BaseLingeringAOE : MonoBehaviour
{
    [SerializeField] protected float Duration = 5f;
    private float timeForVanish;
    [SerializeField] private EnemyType enemyType;
    protected virtual void OnEnable()
    {
        timeForVanish = Time.time + Duration;
    }

    protected virtual void Update()
    {
        if (Time.time > timeForVanish)
        {
            //GO BACK TO POOL
            Disappear();
        }
    }
    protected virtual void Disappear()
    {
        PoolManager.Instance.ReturnObjectToPool(enemyType, gameObject);
    }

}
