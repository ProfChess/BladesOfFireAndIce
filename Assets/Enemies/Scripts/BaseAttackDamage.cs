using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAttackDamage : MonoBehaviour
{
    //Basic Storage for an attack or abilities damage stat
    [Header("Damage Stats")]
    [SerializeField] protected float AttackDamage;
    public float GetDamageNumber() { return AttackDamage; }

}
