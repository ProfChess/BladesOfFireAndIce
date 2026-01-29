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

    //Subscribing to Events
    public void SubscribeToPlayerEvent(Action<PlayerEventContext> BoonEffect, BoonEventType Event)
    {
        switch (Event)
        {
            case BoonEventType.OnNormalAttack: playerAttack.OnNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalEnemyHit: attackEvents.OnEnemyHitNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalDamageEnemyDeath: attackEvents.OnEnemyDeathNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalCriticalHit: attackEvents.OnCriticalHitNormalAttack += BoonEffect; break;
            case BoonEventType.OnAbilityUse: BasePlayerAbility.OnAbilityUse += BoonEffect; break;
            case BoonEventType.OnAbilityDamage: BasePlayerAbility.OnAbilityDamage += BoonEffect; break;
            case BoonEventType.OnAbilityKill: BasePlayerAbility.OnAbilityKill += BoonEffect; break;

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
            case BoonEventType.OnAbilityUse: BasePlayerAbility.OnAbilityUse -= BoonEffect; break;
            case BoonEventType.OnAbilityDamage: BasePlayerAbility.OnAbilityDamage -= BoonEffect; break;
            case BoonEventType.OnAbilityKill: BasePlayerAbility.OnAbilityKill -= BoonEffect; break;

            default: Debug.Log("Incorrect Event Given"); break;
        }
    }


    //Numbers Going Up From Boons
    public void ChangeBonus(StatType stat, float Amount)
    {
        playerStatModding.ApplyBonusStat(stat, Amount);
    }
    public void UndoChange(StatType stat, float Amount)
    {
        playerStatModding.ApplyBonusStat(stat, -Amount);
    }
}

public enum BoonEventType { OnNormalEnemyHit, OnNormalDamageEnemyDeath, OnNormalCriticalHit, 
    OnAbilityUse, OnAbilityDamage, OnAbilityKill, OnNormalAttack}

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
    public StatType stat;
    public float oldValue;
    public float newValue;

    public void Setup(ElementType Ele, StatType Stat, float OldValue, float NewValue)
    {
        Element = Ele;
        stat = Stat;
        oldValue = OldValue;
        newValue = NewValue;
    }
}
