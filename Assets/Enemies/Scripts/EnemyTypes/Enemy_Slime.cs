using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Slime : BaseEnemy
{
    //Anim Toggles/Triggers
    private static readonly int WalkingToggle = Animator.StringToHash("IsWalking");
    private static readonly int RunningToggle = Animator.StringToHash("IsRunning");
    private static readonly int AttackTrig = Animator.StringToHash("AttackTrigger");
    protected override void EnemyIdleState()
    {
        base.EnemyIdleState();
        FlipSpriteInDirection();
        anim.SetBool(WalkingToggle, IsMoving());
        anim.SetBool(RunningToggle, false);
    }
    protected override void EnemyChaseState()
    {
        base.EnemyChaseState();
        FlipSpriteInDirection();
        anim.SetBool(WalkingToggle, false);
        anim.SetBool(RunningToggle, IsMoving());
    }
    protected override bool EnemyAttackState()
    {
        bool didAttack = base.EnemyAttackState();

        if(didAttack)
        {
            anim.SetTrigger(AttackTrig);
        }
        FlipSpriteInDirection();
        anim.SetBool(WalkingToggle, false);
        anim.SetBool(RunningToggle, false);
        return didAttack;
    }
    private void FlipSpriteInDirection()
    {
        Vector2 destination = agent.pathEndPosition;
        EnemySprite.flipX = destination.x < transform.position.x;
    }
}
