
using TreeEditor;
using UnityEditor;
using UnityEngine;

public class Enemy_Slime : BaseEnemy
{
    //Anim Toggles/Triggers
    private static readonly int WalkingToggle = Animator.StringToHash("IsWalking");
    private static readonly int RunningToggle = Animator.StringToHash("IsRunning");
    private static readonly int AttackTrig = Animator.StringToHash("AttackTrigger");
    protected override void EnemyIdleState()
    {
        base.EnemyIdleState();
        FlipSpriteInPathDirection();
        anim.SetBool(WalkingToggle, IsMoving());
        anim.SetBool(RunningToggle, false);
    }
    protected override void EnemyChaseState()
    {
        base.EnemyChaseState();
        FlipSpriteInPathDirection();
        anim.SetBool(WalkingToggle, false);
        anim.SetBool(RunningToggle, IsMoving());
    }
    protected override bool EnemyAttackState()
    {
        bool didAttack = base.EnemyAttackState();

        if(didAttack)
        {
            anim.SetTrigger(AttackTrig);
            SpriteFacePlayerDirection();
        }
        anim.SetBool(WalkingToggle, false);
        anim.SetBool(RunningToggle, false);
        return didAttack;
    }
    protected override void EnemyReturnHomeState()
    {
        base.EnemyReturnHomeState();
        anim.SetBool(RunningToggle, false);
        anim.SetBool(WalkingToggle, IsMoving());
    }
    private void FlipSpriteInPathDirection()
    {
        Vector2 destination = agent.pathEndPosition;
        EnemySprite.flipX = destination.x < transform.position.x;
    }
    private void SpriteFacePlayerDirection()
    {
        EnemySprite.flipX = playerLocation.position.x < transform.position.x;
    }
    protected override void Update()
    {
        base.Update();

        if(isMovementLocked || CurrentEnemyState == EnemyState.Attack)
        {
            SpriteFacePlayerDirection();
        }
        else
        {
            FlipSpriteInPathDirection();
        }
        
    }
}
