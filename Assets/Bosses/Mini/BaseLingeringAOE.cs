using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ElementType { Fire, Ice }
public abstract class BaseLingeringAOE : BaseAttackDamage
{
    public ElementType Element;
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
            gameObject.SetActive(false);
        }
    }


    //Element Effects
    protected abstract void ApplyEffect(GameObject target);                 //Status Effects or Hit sound
    protected abstract void ReactToElement(ElementType IncomingElement);    //Be destroyed or damaged from correct element

}
