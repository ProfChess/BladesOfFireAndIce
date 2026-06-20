using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Archer : BaseEnemy
{
    protected override void EnemyIdleState()
    {
        base.EnemyIdleState();
    }
    protected override void EnemyChaseState()
    {
        base.EnemyChaseState();
    }
    protected override bool EnemyAttackState()
    {
        return base.EnemyAttackState();
    }
}
