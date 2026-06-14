using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_MeleeTowardsPlayer : BaseEnemyAttack_Melee
{
    [SerializeField] private float AttackRange = 1f;
    [SerializeField] private float AnimWaitTime = 0f;
    [SerializeField] private float AttackColActiveDuration = 0.1f;
    public override void Attack()
    {
        if (!canAttack) { return; }

        Vector2 AttackLocation = GetPlayerDirection() * AttackRange;
        AttackCol.offset = AttackLocation;

        StartCoroutine(AttackColActivate());
    }
    private IEnumerator AttackColActivate()
    {
        yield return GameTimeManager.WaitFor(AnimWaitTime);
        AttackCol.enabled = true;
        yield return GameTimeManager.WaitFor(AttackColActiveDuration);
        AttackCol.enabled = false;
    }
}
