using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
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
    [SerializeField] private PlayerStatSetting playerStatModding;
    [SerializeField] private PlayerAttackCalcs attackEvents;
    [SerializeField] private PlayerAttack playerAttack;

    //Subscribing to Events
    public void SubscribeToPlayerEvent(Action<AttackEventDetails> BoonEffect, BoonEventType Event)
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
    public void UnSubscribeFromPlayerEvent(Action<AttackEventDetails> BoonEffect, BoonEventType Event)
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
public class AttackEventDetails
{
    public BaseHealth Target;          //Target Getting Attacked
    public Vector2 AttackOrigin;       //location of Attack 
    public Vector2 Direction;          //Direction of Attack
    public ElementType Element;        //Element of Attack
}