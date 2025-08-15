using System;
using System.Collections.Generic;
using UnityEngine;

public class FlameTankMovement : BaseBoss
{
    [Header("Visuals")]
    [SerializeField] private SpriteRenderer BossSprite;
    [SerializeField] private Animator FireTankAnim;
    private static readonly int MovingBool = Animator.StringToHash("IsMoving");

    protected override void AttackSelection()
    {
        base.AttackSelection();
    }

    //Movement
    protected override void MoveUpdate()
    {
        BossAgent.SetDestination(playerLocation.position);
        if (!ShouldFaceRight())
        {
            BossSprite.flipX = true; //Sprite Faces Right by Default
        }
        else { BossSprite.flipX = false; }

        //Controls Moving Animation
        if (isAttacking) { FireTankAnim.SetBool(MovingBool, false); return; }
        if (BossAgent.remainingDistance - BossAgent.stoppingDistance <= 0)
        {
            FireTankAnim.SetBool(MovingBool, false);
        }
        else { FireTankAnim.SetBool(MovingBool, true); }
    }


    //Agent Creation
    protected override void CreateBossNavAgent()
    {
        base.CreateBossNavAgent();
        BossAgent.stoppingDistance = 3f;
    }

}


