using UnityEngine;

public abstract class BaseAOEHealth : BaseHealth
{
    public ElementType Element;
    private bool hasCollided = false;
    public bool collide => hasCollided;
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
            BasePlayerDamage attackScript = col.GetComponent<BasePlayerDamage>();
            ElementType elementHit = attackScript.GetElement();
            float Damage = attackScript.GetAttackDamage();

            ReactToElement(elementHit, Damage);
        }
    }
    public void DieOnCollision() { hasCollided = true; curHealth = 1; TakeDamage(1); }

}
