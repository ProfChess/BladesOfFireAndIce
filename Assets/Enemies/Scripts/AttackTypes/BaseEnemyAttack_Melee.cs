using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyAttack_Melee : BaseEnemyAttack
{
    [SerializeField] protected Collider2D AttackCol;
    [SerializeField] protected float AnimWaitTime = 0f;
    [SerializeField] protected float AttackColActiveDuration = 0.1f;

    protected IEnumerator AttackColActivate()
    {
        attackInProgress = true;
        canAttack = false;
        yield return GameTimeManager.WaitFor(AnimWaitTime);
        AttackCol.enabled = true;
        yield return GameTimeManager.WaitFor(AttackColActiveDuration);
        AttackCol.enabled = false;
        attackInProgress = false;
        yield return BasicAttackCooldown();
    }
}
