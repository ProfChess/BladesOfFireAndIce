using System;
using UnityEngine;

public class PlayerEffectSubscriptionManager : MonoBehaviour
{
    //Accessable Instance
    public static PlayerEffectSubscriptionManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    //Events Locations
    [SerializeField] private PlayerStatSetting playerStatModding;
    [SerializeField] private PlayerAttackCalcs attackEvents;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerHealth playerHealth;

    //Subscribing to Events
    public void SubscribeToPlayerEvent(Action<PlayerEventContext> BoonEffect, BoonEventType Event)
    {
        switch (Event)
        {
            case BoonEventType.OnNormalAttack: playerAttack.OnNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalEnemyHit: attackEvents.OnEnemyHitNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalDamageEnemyDeath: attackEvents.OnEnemyDeathNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalCriticalHit: attackEvents.OnCriticalHitNormalAttack += BoonEffect; break;
            case BoonEventType.OnAbilityUse: PlayerAbilities.OnAbilityUse += BoonEffect; break;
            case BoonEventType.OnAbilityDamage: BaseEffectSpawn.OnAbilityDamage += BoonEffect; break;
            case BoonEventType.OnAbilityKill: BaseEffectSpawn.OnAbilityKill += BoonEffect; break;
            case BoonEventType.OnHealthChange: playerHealth.PlayerHealthChange += BoonEffect; break;

            default: Debug.Log("Incorrect Event Given"); break;
        }
    }

    //Unsubscribing From Events
    public void UnSubscribeFromPlayerEvent(Action<PlayerEventContext> BoonEffect, BoonEventType Event)
    {
        switch (Event)
        {
            case BoonEventType.OnNormalAttack: playerAttack.OnNormalAttack -= BoonEffect; break;
            case BoonEventType.OnNormalEnemyHit: attackEvents.OnEnemyHitNormalAttack -= BoonEffect; break;
            case BoonEventType.OnNormalDamageEnemyDeath: attackEvents.OnEnemyDeathNormalAttack -= BoonEffect; break;
            case BoonEventType.OnNormalCriticalHit: attackEvents.OnCriticalHitNormalAttack -= BoonEffect; break;
            case BoonEventType.OnAbilityUse: PlayerAbilities.OnAbilityUse -= BoonEffect; break;
            case BoonEventType.OnAbilityDamage: BaseEffectSpawn.OnAbilityDamage -= BoonEffect; break;
            case BoonEventType.OnAbilityKill: BaseEffectSpawn.OnAbilityKill -= BoonEffect; break;

            default: Debug.Log("Incorrect Event Given"); break;
        }
    }


    //Numbers Going Up From Relics
    public void AddBonus(StatType stat, float AmountAsPercentage)
    {
        playerStatModding.ApplyBonusStat(stat, AmountAsPercentage);
    }
    public void RemoveBonus(StatType stat, float AmountAsPercentage)
    {
        playerStatModding.ApplyBonusStat(stat, -AmountAsPercentage);
    }

    //State Checking For Relics
    public StatChangeEventContext GetPlayerState(PlayerStateCheckType StateCheckType)
    {
        if (StateCheckType == PlayerStateCheckType.None) return null;

        //Create Pickup Context
        StatChangeEventContext tempCtx = new();
        switch (StateCheckType)
        {
            case PlayerStateCheckType.Health: 
                tempCtx.Setup(PlayerController.PlayerAttackForm, 
                    playerHealth.CurrentHealth, 
                    playerHealth.CurrentHealth, 
                    playerHealth.GetPlayerMaxHealth);
                break;
        }
        return tempCtx;
    }
}

public enum BoonEventType 
{ 
    OnNormalAttack = 0, OnNormalEnemyHit = 1, OnNormalDamageEnemyDeath = 2, OnNormalCriticalHit = 3, 
    OnAbilityUse = 10, OnAbilityDamage = 11, OnAbilityKill = 12,
    OnHealthChange = 20,
}
public enum PlayerStateCheckType { None = 0, Health = 1}

//Attack Information Given to Each Boon Effect
public class PlayerEventContext
{
    public PlayerController player => GameManager.Instance.getPlayer().GetComponent<PlayerController>();
    public Vector2 Direction;          //Direction of Attack or Mouse Location upon Event Fire
    public ElementType Element;        //Element of Attack or Current Stance of Player

    public void Setup(ElementType Ele, Vector2 Dir)
    {
        Element = Ele;
        Direction = Dir;
    }
}
public class AttackEventContext : PlayerEventContext
{
    public BaseHealth Target;          //Target Getting Attacked
    public Vector2 AttackBoxOrigin;    //Location of Hitbox

    public void Setup(ElementType Ele, Vector2 Dir, BaseHealth Enemy, Vector2 AttackBox)
    {
        Element = Ele;
        Direction = Dir;
        Target = Enemy;
        AttackBoxOrigin = AttackBox;
    }
}
public class StatChangeEventContext : PlayerEventContext
{
    public float oldValue;
    public float newValue;
    public float maxValue;

    public void Setup(ElementType Ele, float OldValue, float NewValue, float MaxValue)
    {
        Element = Ele;
        oldValue = OldValue;
        newValue = NewValue;
        maxValue = MaxValue;
    }
}


