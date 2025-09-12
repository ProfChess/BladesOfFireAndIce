using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBossAttack : BaseAttackDamage
{
    protected BaseBoss BossRef;

    public virtual void StartAttack(BossAttackOption AttackOption)
    {
        //INPUT ATTACK LOGIC

    }
    protected virtual void Start()
    {
        Transform tempObject = GetComponent<Transform>();
        BossRef = tempObject.GetComponentInParent<BaseBoss>();
    }
}
