using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoonManager : MonoBehaviour
{
    //Accessable Instance
    public static PlayerBoonManager Instance { get; private set; }
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
    [SerializeField] private PlayerStatSetting PlayerStatModding;
    [SerializeField] private PlayerAttackCalcs AttackEvents;
    
    //Subscribing to Events
    public void SubscribeToDamageEvent(Action<ElementType, BaseHealth> BoonEffect, BoonEventType Event)
    {
        switch (Event)
        {
            case BoonEventType.OnNormalEnemyHit: AttackEvents.OnEnemyHitNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalDamageEnemyDeath: AttackEvents.OnEnemyDeathNormalAttack += BoonEffect; break;
            case BoonEventType.OnNormalCriticalHit: AttackEvents.OnCriticalHitNormalAttack += BoonEffect; break;
            case BoonEventType.OnAbilityDamage: BasePlayerAbility.OnAbilityDamage += BoonEffect; break;
            case BoonEventType.OnAbilityKill: BasePlayerAbility.OnAbilityKill += BoonEffect; break;

            default: Debug.Log("Incorrect Event Given"); break;
        }
    }
    public void SubscribeToElementEvent(Action<ElementType> BoonEffect, BoonEventType Event)
    {
        switch(Event)
        {
            case BoonEventType.OnAbilityUse: BasePlayerAbility.OnAbilityUse += BoonEffect; break;
            
            
            default: Debug.Log("Incorrect Event Given"); break;
        }
    }


    //Unsubscribing From Events
    public void UnSubscribeFromDamageEvent(Action<ElementType, BaseHealth> BoonEffect, BoonEventType Event)
    {
        switch (Event)
        {
            case BoonEventType.OnNormalEnemyHit: AttackEvents.OnEnemyHitNormalAttack -= BoonEffect; break;
            case BoonEventType.OnNormalDamageEnemyDeath: AttackEvents.OnEnemyDeathNormalAttack -= BoonEffect; break;
            case BoonEventType.OnNormalCriticalHit: AttackEvents.OnCriticalHitNormalAttack -= BoonEffect; break;
            case BoonEventType.OnAbilityDamage: BasePlayerAbility.OnAbilityDamage -= BoonEffect; break;
            case BoonEventType.OnAbilityKill: BasePlayerAbility.OnAbilityKill -= BoonEffect; break;

            default: Debug.Log("Incorrect Event Given"); break;
        }
    }
    public void UnSubscribeFromElementEvent(Action<ElementType> BoonEffect, BoonEventType Event)
    {
        switch (Event)
        {
            case BoonEventType.OnAbilityUse: BasePlayerAbility.OnAbilityUse -= BoonEffect; break;


            default: Debug.Log("Incorrect Event Given"); break;
        }
    }


    //Numbers Going Up From Boons
    public void ChangeBonus(StatType stat, float Amount)
    {
        PlayerStatModding.ApplyBonusStat(stat, Amount);
    }
    public void UndoChange(StatType stat, float Amount)
    {
        PlayerStatModding.ApplyBonusStat(stat, -Amount);
    }
}

public enum BoonEventType { OnNormalEnemyHit, OnNormalDamageEnemyDeath, OnNormalCriticalHit, 
    OnAbilityUse, OnAbilityDamage, OnAbilityKill}
