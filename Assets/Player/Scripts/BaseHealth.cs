using UnityEngine;

public abstract class BaseHealth : MonoBehaviour
{
    //BASE CLASS FOR THINGS WITH HEALTH
    [Header("Health")]
    [SerializeField] protected float MaxHealth;
    [SerializeField] protected float curHealth = 0;
    public float CurrentHealth => curHealth;

    [Header("Collision")]
    [SerializeField] private float CollisionDamage = 1f;
    [SerializeField] private bool HasCollisionDamage = false;
    public bool DoesCollisionDamage => HasCollisionDamage;

    private bool StorageCollisionBool;

    //Object Takes Damage -> sets to 0 if below
    protected virtual void TakeDamage(float Damage) 
    {
        curHealth -= Damage;
        if (curHealth <= 0) { curHealth = 0;  HasCollisionDamage = false; }
    }
    public virtual float GetCollisionDamage() { if (HasCollisionDamage) { return CollisionDamage; } else return 0f; }
    public void SetCollisionDamage(bool Col) { HasCollisionDamage = Col; }
    private void Awake()
    {
        StorageCollisionBool = HasCollisionDamage;
    }
    protected virtual void Start()
    {
        curHealth = MaxHealth;
    }
    private void OnEnable()
    {
        HasCollisionDamage = StorageCollisionBool;
    }


}
