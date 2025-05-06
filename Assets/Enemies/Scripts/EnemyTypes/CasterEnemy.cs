using System.Collections;
using UnityEngine;

public class CasterEnemy : BaseEnemy
{
    //ANIMATION SAVES
    //Triggers
    private static readonly int AwakeTrigger = Animator.StringToHash("WakeUp");
    //States
    private static readonly int SleepState = Animator.StringToHash("Sleeping");
    private static readonly int MagicAttackState = Animator.StringToHash("CasterMagicAttack");
    private static readonly int TeleportStart = Animator.StringToHash("CasterDisappear");
    private static readonly int TeleportEnd = Animator.StringToHash("CasterAppear");


    //Waiting 
    private bool isWaiting = false;
    [SerializeField] private float WaitTime;
    private float Timer;
    
    private bool isAwake = false;   //Triggers enemy waking up

    //Teleporting
    private bool CanTeleportAfterCast = true;
    private bool isTeleportingAfterCast = false;
    private bool isTeleportingRegular = false;

    //Unique Attack (Cast Attack)
    private CastAttack UniqueAttack;
    [Header("Cast Attack Settings")]
    [SerializeField] private float MagicSlashCDTime;
    [SerializeField] private float MagicDamage;
    [SerializeField] private float MagicProjectileSpeed;
    [SerializeField] private float MagicAttackRange;
    [SerializeField] private float MagicSlashTeleportCD;


    private bool canCast = true;

    protected override void Start()
    {
        base.Start();
        UniqueAttack = GetComponent<CastAttack>();
    }
    protected override void EnemyIdleState()
    {
        if (isAwake)
        {
            base.EnemyIdleState();
            IdleWanderThenWait(ref isWaiting, ref Timer, WaitTime);
        }
    }

    //CHASE STATE
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
            if (canCast)
            {
                anim.Play(MagicAttackState);
            }

            
            //If can't Teleport, Move a Short Distance Away --> Repeat Attack


        }
    }
    public void AttemptTeleport() 
    {
        EnemySprite.sortingOrder = -3;
        if (isTeleportingAfterCast) 
        { 
            EnemyMovementComponent.ChaseMove(agent, playerLocation, 0f, 0f);
            StartCoroutine(TravelDelayWait());
        }
        else if (isTeleportingRegular)
        {

        }
    }
    //Cooldowns
    //Calls
    public void BeginMagicAttackCD() 
    { 
        StartCoroutine(MagicAttackCooldown());
        if (CanTeleportAfterCast)
        {
            isTeleportingAfterCast = true;
            anim.Play(TeleportStart);
        }
    }
    public void BeginMATeleportCD() 
    {  
        StartCoroutine(MATeleportCooldown()); 
    }
    //Coroutines
    private IEnumerator MagicAttackCooldown()
    {
        canCast = false;
        yield return new WaitForSeconds(MagicSlashCDTime);
        canCast = true;
    }
    private IEnumerator MATeleportCooldown()
    {
        CanTeleportAfterCast = false;
        yield return new WaitForSeconds(MagicSlashTeleportCD);
        CanTeleportAfterCast = true;
    }
    private IEnumerator TravelDelayWait()
    {
        yield return new WaitForSeconds(0.3f);
        EnemySprite.sortingOrder = 2;
        anim.Play(TeleportEnd);
    }


    //ATTACK STATE  
    protected override void EnemyAttackState()
    {
        //Perform Regular Attack, Then Teleport somewhere x distance away from player
        // (Seperate Cooldowns for Teleport Timers)
        base.EnemyAttackState(); 
    }
    public void SpawnMagicAttack()
    {
        UniqueAttack.Attack(MagicDamage, MagicAttackRange, 0, MagicProjectileSpeed, playerLocation);
    }


    //Death Logic
    protected override void CustomEnemyDeathLogic()
    {
        isAwake = false;
        isWaiting = false;
        anim.Play(SleepState, 0);
    }
}
