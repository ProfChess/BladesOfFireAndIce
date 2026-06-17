using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_MeleeTowardsPlayer : BaseEnemyAttack_Melee
{
    [SerializeField] private float AttackRange = 1f;

    public override void Attack()
    {
        if (!canAttack) { return; }
        if (attackInProgress) { return; }

        Vector2 AttackLocation = GetPlayerDirection() * AttackRange;
        AttackCol.offset = AttackLocation;

        StartCoroutine(AttackColActivate());
    }

}
