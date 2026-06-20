using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyAttack_Melee : BaseEnemyAttack
{
    [SerializeField] protected Collider2D AttackCol;
    [Tooltip("Amount of Time Leading Up to Attack Will Land")]
    [SerializeField] protected float AnimStartWaitTime = 0f;
    [Tooltip("Time After Attack Lands for Enemy to Pause Before Exiting Attack State")]
    [SerializeField] protected float AnimEndWaitTime = 0f;
    [SerializeField] protected float AttackColActiveDuration = 0.1f;

    protected virtual IEnumerator AttackColActivate()
    {
        attackInProgress = true;
        canAttack = false;
        //Lock Movement if Necessary
        if (Enemy != null && pausesEnemyMovement) { Enemy.LockEnemyMovement(true); }

        yield return GameTimeManager.WaitFor(AnimStartWaitTime);
        AttackCol.enabled = true;
        yield return GameTimeManager.WaitFor(AttackColActiveDuration);
        AttackCol.enabled = false;
        yield return GameTimeManager.WaitFor(AnimEndWaitTime);
        attackInProgress = false;

        if (Enemy != null && pausesEnemyMovement)
        {
            Enemy.LockEnemyMovement(false);
        }
        yield return BasicAttackCooldown();
    }
}
