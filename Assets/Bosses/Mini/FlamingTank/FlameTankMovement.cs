using System;
using System.Collections.Generic;
using UnityEngine;

public class FlameTankMovement : BaseBoss
{
    [SerializeField] private SpriteRenderer BossSprite;
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
    }

}


