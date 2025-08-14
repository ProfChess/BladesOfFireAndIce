using UnityEngine;

public enum ElementType { Fire, Ice }
public abstract class BaseLingeringAOE : MonoBehaviour
{
    [SerializeField] protected float Duration = 5f;
    private float timeForVanish;

    protected virtual void Start()
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
        gameObject.SetActive(false);
    }


}
