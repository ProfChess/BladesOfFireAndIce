using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemy : BaseEnemy
{
    
    private bool isAwake = false;   //Triggers enemy waking up
    protected override void EnemyIdleState()
    {
        if (isAwake)
        {
            base.EnemyIdleState();

        }
    }

    protected override void EnemyChaseState()
    {
        //Wakes up and begins attacking player
        if (!isAwake)
        {
            isAwake = true;
            anim.SetTrigger("WakeUp");
        }
        else
        {
            //Sets Anim When not Attacking
            anim.SetBool("IsWalking", false);
            anim.SetBool("IsRunning", true);

            //If Magic Attack is Off Cooldown --> Cast
            //Check if Player is Close
            //If Yes and can Teleport, Teleport near wall
            //If No or Can't Teleport, Move a Short Distance Away --> Repeat Attack


        }
    }

    protected override void EnemyAttackState()
    {
        //Perform Regular Attack, Then Teleport somewhere x distance away from player
        // (Seperate Cooldowns for Teleport Timers)
        base.EnemyAttackState(); 
    }

}
