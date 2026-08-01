
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

        if (!isActive) { return; }
        FlipSpriteInPathDirection();
    }
    protected override void EnemyChaseState()
    {
        base.EnemyChaseState();
        FlipSpriteInPathDirection();
    }
    protected override bool EnemyAttackState()
    {
        bool didAttack = base.EnemyAttackState();

        if(didAttack)
        {
            anim.SetTrigger(AttackTrig);
            SpriteFacePlayerDirection();
        }
        return didAttack;
    }
    protected override void OnStateEnterAttack()
    {
        base.OnStateEnterAttack();
        anim.SetBool(WalkingToggle, false);
        anim.SetBool(RunningToggle, false);
    }
    protected override void EnemyReturnHomeState()
    {
        base.EnemyReturnHomeState();
        
        if (CurrentEnemyState != EnemyState.ReturnHome) { return; }

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
        UpdateMovementAnimation();
    }
    protected void UpdateMovementAnimation()
    {
        bool moving = IsMoving();
        switch (CurrentEnemyState)
        {
            case EnemyState.Idle:
                anim.SetBool(WalkingToggle, moving);
                anim.SetBool(RunningToggle, false);
                break;
            case EnemyState.Chase:
                anim.SetBool(WalkingToggle, false);
                anim.SetBool(RunningToggle, moving);
                break;
            case EnemyState.Attack:
                anim.SetBool(WalkingToggle, false);
                anim.SetBool(RunningToggle, false);
                break;
            case EnemyState.ReturnHome:
                break;
        }
    }
}
