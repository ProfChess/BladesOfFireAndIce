using System.Collections;
using UnityEngine;

public class ChargerEnemy : BaseEnemy
{
    //ANIMATION STRINGS
    //States
    private static readonly int SpinAttackStart = Animator.StringToHash("ChargerSpinAttackIntro");
    private static readonly int NormalAttack = Animator.StringToHash("ChargerNormalAttack");

    [Tooltip("Controls How Long (Seconds) the Enemy Waits at Each Patrol Point")]
    [SerializeField] private float TimeBetweenPatrols;
    private float WaitTimeRemaining = 0;
    private bool isWaiting = false;

    //Extra Attack
    private ChargeAttack chargeAttack;
    [Header("Charge Attack Settings")]
    [Tooltip("Controls Cooldown for Charge Attack")]
    [SerializeField] private int ChargeCooldownTime;
    [Tooltip("Controls Damage of Charge Attack")]
    [SerializeField] private float chargeDamage;
    [Tooltip("Controls How Long The Enemy Spins")]
    [SerializeField] private float chargeDuration;
    [Tooltip("Controls Speed of Enemy as They Move While Spinning")]
    [SerializeField] private float chargeSpeed;
    [Tooltip("Controls Range of Enemy Charge Attack")]
    [SerializeField] private float chargeRange;

    private bool isCharging = false;
    private bool CanChargeAttack = true;

    private float TempAttackRange;
    private const float DisableRange = -1f;
    protected override void Start()
    {
        base.Start();
        chargeAttack = GetComponent<ChargeAttack>();
        TempAttackRange = AttackRange;
    }

    //Enemy Will Patrol Between 2 Points of its Starting Room in its Idle State
    protected override void EnemyIdleState()
    {
        IdleWanderThenWait(ref isWaiting, ref WaitTimeRemaining, TimeBetweenPatrols);
        
        base.EnemyIdleState();
    }


    //Enemy Will Use Their Spin Attack On Cooldown When Player is Within Chase Range
    protected override void EnemyChaseState()
    {
        if (CanChargeAttack)
        {
            if (!isCharging)
            {
                ChargeApproachState();
                isCharging=true;
            }
        }
        else if (!isCharging)
        {
            NormalApproachState();
        }
    }
    
    //Approaches Player when Charge is on Cooldown
    private void NormalApproachState()
    {
        base.EnemyChaseState();
        EnemyMovementComponent.ChaseMove(agent, playerLocation, ChaseSpeed, AttackOffset);
    }

    //Charges At Player
    private void ChargeApproachState()
    {
        AttackRange = DisableRange;

        agent.speed = chargeSpeed;
        Vector2 Direction = GetPlayerDirection().normalized * chargeRange;
        Vector2 TargetPosition = new Vector2(transform.position.x, transform.position.y) + Direction;
        anim.Play(SpinAttackStart);
        chargeAttack.Attack(chargeDamage, chargeDuration, 0, 0, playerLocation);
        agent.SetDestination(TargetPosition);
    }
    private IEnumerator ChargeAttackCooldown() //Enemy Cannot Charge for Cooldown Duration
    {
        CanChargeAttack = false;
        yield return new WaitForSeconds(ChargeCooldownTime);
        CanChargeAttack = true;
    }
    public void StartChargeCooldown() //End of Charge Attack
    {
        StartCoroutine(ChargeAttackCooldown()); isCharging = false; AttackRange = TempAttackRange;
    }

    //Enemy Will Use Normal Attack When in Attack Range (Close)
    protected override void EnemyAttackState()
    {
        base.EnemyAttackState();
        anim.SetBool(Walking, false); anim.SetBool(Running, false);
        if (canAttack) { canAttack = false; anim.Play(NormalAttack); }
    }


}
