using System;
using System.Collections;
using UnityEngine;

public class BossShieldCreate : BaseHealth
{
    [Header("Shield Stats")]
    [SerializeField] private float ShieldStrength = 100f;
    [SerializeField] private float ShieldDuration = 6f;
    [SerializeField] private bool StunnedOnBreak = true;
    [SerializeField] private float ShieldStunDuration = 3f;
    //Getters
    public float GetShieldStunDuration => ShieldStunDuration;
    public bool GetStunnedOnBreak => StunnedOnBreak;

    //Other Shield
    private float CurrentShieldStrength;
    private ElementType ShieldElement;
    private bool MainHitboxCollisionBool;

    //Events
    public event Action ShieldBroken;

    //Coroutines
    private Coroutine ShieldLifeRoutine;

    //Layers
    private static class Layers
    {
        public static readonly int ShieldLayer = LayerMask.NameToLayer("ShieldedLayer");
        public static readonly int StartLayer = LayerMask.NameToLayer("Enemy");
    }

    [Header("References")]
    [SerializeField] private BaseHealth MainHitbox;
    [SerializeField] private CapsuleCollider2D ShieldHitbox;

    private void Start()
    {
        ShieldHitbox.enabled = false;
        CurrentShieldStrength = ShieldStrength;
        MainHitboxCollisionBool = MainHitbox.DoesCollisionDamage;
    }
    public void CreateShield(ElementType Element) //Assign Shield element, turn off regular collisions, activate col
    {
        ShieldElement = Element;
        MainHitbox.gameObject.layer = Layers.ShieldLayer;
        MainHitbox.SetCollisionDamage(false);

        ShieldHitbox.enabled = true;
        CurrentShieldStrength = ShieldStrength;

        //Give Lifetime
        if (ShieldLifeRoutine == null) { ShieldLifeRoutine = StartCoroutine(ShieldLifeTime()); }
    }
    private void DeactivateShield() //Turn off Shield, reactivate normal collisions
    {
        if (ShieldLifeRoutine != null) { StopCoroutine(ShieldLifeRoutine); ShieldLifeRoutine = null; }
        MainHitbox.gameObject.layer = Layers.StartLayer;
        ShieldHitbox.enabled = false;
        MainHitbox.SetCollisionDamage(MainHitboxCollisionBool);
    }
    private IEnumerator ShieldLifeTime()
    {
        yield return new WaitForSeconds(ShieldDuration);
        DeactivateShield();
        ShieldLifeRoutine = null;
    }


    //Check if Can Damage Shield, Then check if shield dead
    private void ShieldDamaged(float damage, ElementType Element)
    {
        if (ShieldElement == Element) { return; }
        CurrentShieldStrength -= damage;
        if (CurrentShieldStrength <= 0) { DeactivateShield(); ShieldBroken?.Invoke(); }
    }

    //Damage Detection
    private void OnTriggerEnter2D(Collider2D col) //Detect Player Damage, Pass Damage Number and Element Type into Function
    {
        if (CurrentShieldStrength <= 0) { return; }
        if (col.CompareTag("PlayerAttack"))
        {
            BasePlayerDamage PlayerDamageCode = col.GetComponent<BasePlayerDamage>();
            ElementType PlayerElement = PlayerDamageCode.GetElement();
            float PlayerDamage = PlayerDamageCode.GetDamageNumber();
            ShieldDamaged(PlayerDamage, PlayerElement);
        }
    }
}
