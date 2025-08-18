using UnityEngine;

public abstract class BaseHealth : MonoBehaviour
{
    //BASE CLASS FOR THINGS WITH HEALTH
    [Header("Health")]
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float curHealth = 0;

    [Header("Collision")]
    [SerializeField] private float CollisionDamage = 1f;
    [SerializeField] private bool HasCollisionDamage = false;
    private bool StorageCollisionBool;

    //Object Takes Damage -> sets to 0 if below
    protected virtual void TakeDamage(float Damage) 
    {
        curHealth -= Damage;
        if (curHealth <= 0) { curHealth = 0;  HasCollisionDamage = false; }
    }
    public float GetCollisionDamage() { if (HasCollisionDamage) { return CollisionDamage; } else return 0f; }

    private void Start()
    {
        curHealth = MaxHealth;
        StorageCollisionBool = HasCollisionDamage;
    }
    private void OnEnable()
    {
        HasCollisionDamage = StorageCollisionBool;
    }
}
