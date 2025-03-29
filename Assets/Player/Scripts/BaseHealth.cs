using UnityEngine;

public abstract class BaseHealth : MonoBehaviour
{
    //BASE CLASS FOR THINGS WITH HEALTH
    [SerializeField] private float MaxHealth;
    public float curHealth { get; private set; }

    //Object Takes Damage -> sets to 0 if below
    protected void TakeDamage(float Damage) 
    {
        curHealth -= Damage;
        if (curHealth < 0) { curHealth = 0; }
    }

    private void Start()
    {
        curHealth = MaxHealth;
    }
}
