using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackDamage : BaseDamageDetection
{
    //Basic Storage for an attack or abilities damage stat
    [Header("Damage Stats")]
    [SerializeField] protected float AttackDamage;
    public override float GetDamageNumber() { return AttackDamage; }

}
