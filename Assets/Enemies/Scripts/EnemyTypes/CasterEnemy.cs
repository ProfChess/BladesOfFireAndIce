using System;
using System.Collections;
using UnityEngine;

public class CasterEnemy : BaseEnemy
{
    //ANIMATION SAVES
    //Triggers
    private static readonly int AwakeTrigger = Animator.StringToHash("WakeUp");
    private static readonly int CastingTrigger = Animator.StringToHash("IsCasting");
    private static readonly int AttackingTrigger = Animator.StringToHash("IsAttacking");
    private static readonly int TeleportingTrigger = Animator.StringToHash("IsTeleporting");
    private static readonly int AppearingTrigger = Animator.StringToHash("IsAppearing");

    //States
    private static readonly int SleepState = Animator.StringToHash("Sleeping");
    private static readonly int FallAsleep = Animator.StringToHash("CasterFallAsleep");

    //Waiting 
    private bool isWaiting = false;
    [SerializeField] private float WaitTime;
    private float Timer;
    
    private bool isAwake = false;   //Triggers enemy waking up

    //Teleporting
    public bool CanTeleportAfterCast = true;
    public bool CanTeleportRegular = true;

    //Unique Attack (Cast Attack)
    private CastAttack UniqueAttack;
    [Header("Cast Attack Settings")]
    [SerializeField] private float MagicSlashCDTime;
    [SerializeField] private float MagicDamage;
    [SerializeField] private float MagicProjectileSpeed;
    [SerializeField] private float MagicAttackRange;
    [SerializeField] private float MagicSlashTeleportCD;
    [SerializeField] private float RegularTeleportCD;
    public bool canCast = true;

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
            isAwake = false;
            anim.Play(FallAsleep);
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
                StartCoroutine(Cooldown(MagicSlashCDTime, val => canCast = val));
                base.EnemyChaseState();
                anim.SetTrigger(CastingTrigger);
            }
            //Otherwise teleport the same way if possible
            else if (GetPlayerDistance().magnitude <= AttackRange + 2 && CanTeleportAfterCast) 
            {
                StartCoroutine(Cooldown(MagicSlashTeleportCD, val => CanTeleportAfterCast = val));
                anim.SetTrigger(TeleportingTrigger);
            }

        }
    }
    public void TeleportAfterCast() 
    {
        EnemySprite.sortingOrder = -3;
        EnemyMovementComponent.IdleMove(agent, 0f);
        StartCoroutine(TravelDelayWait(0.5f));
    }
    public void TeleportRegular()
    {
        EnemySprite.sortingOrder = -3;
        EnemyMovementComponent.ChaseMove(agent, playerLocation, 0f, 0f);
        StartCoroutine(TravelDelayWait(0.2f));
    }
    //Cooldowns
    //Calls
    public void ConsiderTeleportAfterCast() 
    {
        if (GetPlayerDistance().magnitude <= AttackRange + 2 && CanTeleportAfterCast)
        {
            StartCoroutine(Cooldown(MagicSlashTeleportCD, val => CanTeleportAfterCast = val));
            anim.SetTrigger(TeleportingTrigger);
        }
    }

    //Coroutines
    
    private IEnumerator TravelDelayWait(float time)
    {
        yield return new WaitForSeconds(time);
        EnemySprite.sortingOrder = 2;
        anim.SetTrigger(AppearingTrigger);
        base.EnemyAttackState();
    }
    private IEnumerator Cooldown(float CDTime, Action<bool> SetBool)
    {
        SetBool(false);
        yield return new WaitForSeconds(CDTime);
        SetBool(true);
    }


    //ATTACK STATE  
    protected override void EnemyAttackState()
    {
        //Perform Regular Attack, Then Teleport somewhere x distance away from player
        // (Seperate Cooldowns for Teleport Timers)

        if (canAttack)
        {
            StartCoroutine(BasicAttackCooldown());
            base.EnemyAttackState();
            anim.SetTrigger(AttackingTrigger);
        }
        else if (CanTeleportRegular)
        {
            StartCoroutine(Cooldown(RegularTeleportCD, val => CanTeleportRegular = val));
            anim.SetTrigger(TeleportingTrigger);
        }
        
    }
    //Attacks
    public void SpawnMagicAttack()
    {
        UniqueAttack.Attack(MagicDamage, MagicAttackRange, 0, MagicProjectileSpeed, playerLocation);
    }
    public void NormalAttack() { EnemyAttackComponent.Attack(AttackDamage, 0f, 0, 0f, playerLocation); }

    //Death Logic
    protected override void CustomEnemyDeathLogic()
    {
        isAwake = false;
        isWaiting = false;
        anim.Play(SleepState, 0);
    }
}
