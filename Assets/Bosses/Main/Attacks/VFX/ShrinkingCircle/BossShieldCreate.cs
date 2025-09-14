using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldCreate : BaseHealth
{
    [Header("Shield Stats")]
    [SerializeField] private float ShieldStrength = 100f;
    private float CurrentShieldStrength;
    private ElementType ShieldElement;
    private bool MainHitboxCollisionBool;

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
    }
    private void DeactivateShield() //Turn off Shield, reactivate normal collisions
    {
        MainHitbox.gameObject.layer = Layers.StartLayer;
        ShieldHitbox.enabled = false;
        MainHitbox.SetCollisionDamage(MainHitboxCollisionBool);
    }

    //Check if Can Damage Shield, Then check if shield dead
    private void ShieldDamaged(float damage, ElementType Element)
    {
        if (ShieldElement == Element) { return; }
        CurrentShieldStrength -= damage;
        if (CurrentShieldStrength <= 0) { DeactivateShield(); }
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
