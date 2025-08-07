using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAOEHealth : BaseHealth
{
    public ElementType Element;

    //Be destroyed or damaged from correct element
    protected virtual void ReactToElement(ElementType IncomingElement, float Damage)
    {
        if (IncomingElement != Element)
        {
            TakeDamage(Damage);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerAttack"))
        {
            PlayerAttackCalcs attackScript = col.GetComponent<PlayerAttackCalcs>();
            ElementType elementHit = attackScript.GetElementDamageType();
            float Damage = attackScript.GetDamageWithoutCrit();

            ReactToElement(elementHit, Damage);
        }
    }

}
