using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerDamage : BaseAttackDamage
{
    //Ability Attack Forms
    [Tooltip("Ice or Fire")]
    [SerializeField] protected ElementType Element;
    [Tooltip("Is the attack damage an ability or not")]
    [SerializeField] protected PlayerAttackType AttackType;
    
    public enum PlayerAttackType { NormalAttack, Ability}

    public ElementType GetElement()
    {
        return AttackType == PlayerAttackType.Ability ? Element : PlayerSwitchElements.PlayerAttackForm;
    }
    public PlayerAttackType AttackElement => AttackType;
    public virtual float GetAttackDamage(BaseHealth EnemyHealth) {  return AttackDamage; }
}
