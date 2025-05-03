using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterEnemy : BaseEnemy
{
    //ANIMATION SAVES
    //Triggers
    private static readonly int AwakeTrigger = Animator.StringToHash("WakeUp");
    //States
    private static readonly int SleepState = Animator.StringToHash("Sleeping");


    //Waiting 
    private bool isWaiting = false;
    [SerializeField] private float WaitTime;
    private float Timer;
    
    private bool isAwake = false;   //Triggers enemy waking up

    //Teleporting
    private bool CanTeleport = true;

    protected override void EnemyIdleState()
    {
        if (isAwake)
        {
            base.EnemyIdleState();
            IdleWanderThenWait(ref isWaiting, ref Timer, WaitTime);
        }
    }

    protected override void EnemyChaseState()
    {
        //Wakes up and begins attacking player
        if (!isAwake)
        {
            isAwake = true;
            anim.SetTrigger(AwakeTrigger);
            GetComponent<Teleport>().Create();
        }
        else
        {
            //Sets Anim When not Attacking
            anim.SetBool(Walking, false);
            anim.SetBool(Running, true);

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



    //Death Logic
    protected override void CustomEnemyDeathLogic()
    {
        isAwake = false;
        isWaiting = false;
        anim.Play(SleepState, 0);
    }
}
