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
        BossRef = gameObject.transform.parent?.GetComponentInParent<BaseBoss>();
    }
}
