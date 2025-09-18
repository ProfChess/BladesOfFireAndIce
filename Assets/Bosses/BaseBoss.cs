using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class BossAttackOption
{
    [Tooltip("Name of Attack")]
    public BossAttackType AttackType;
    [Tooltip("Chance Attack is Selected When Within Range")]
    public float SelectionChance;
    public float Duration;
    [Tooltip("Cooldown of Attack")]
    public float AttackCooldown;
    public bool OnCooldown = false;
    [Tooltip("Distance to Player to Trigger Attack")]
    public float AttackRangeTrigger;
    public bool DistanceIsMinimum;  //true -> Greater or Equal to Range | false -> Less Than Range
    public BaseBossAttack AttackRef;

    public void CallAttack()
    {
        if (AttackRef != null && !OnCooldown)
        {
            AttackRef.StartAttack(this);
        }
    }
}
public enum BossAttackType
{
    None, 
    BossAttack1, BossAttack2, BossAttack3, 
    BossAttack4, BossAttack5, BossAttack6, 
    BossAttack7, BossAttack8, BossAttack9
}
public abstract class BaseBoss : MonoBehaviour
{
    //State Updates
    private float UpdateDelay = 0.2f;
    private float nextUpdateCheck = 0f;

    //Movement
    [Header("Movement")]
    [SerializeField] protected float MoveSpeed;
    protected NavMeshAgent BossAgent;
    public NavMeshAgent GetAgent() { return BossAgent; }
    private bool isStunned = false;
    private Coroutine StunnedRoutine;

    //Attacking
    [Header("Attacking")]
    [SerializeField] protected bool isAttacking = false;
    protected float attackTimer;
    protected Transform playerLocation;
    protected Coroutine AttackingDuration;


    [Header("References")]
    [SerializeField] private BossHealth HealthScript;
    
    //List
    [Header("List of Boss Attacks")]
    [SerializeField] private List<BossAttackOption> BossAttacks = new List<BossAttackOption>();
    private Dictionary<BossAttackType, BossAttackOption> AttackDictionary =
        new Dictionary<BossAttackType, BossAttackOption>();


    protected virtual void Start()
    {
        playerLocation = GameManager.Instance.getPlayer().transform;
        foreach (BossAttackOption Entry in BossAttacks) //Create Dictionary of Attacks for Easy Acces
        {
            AttackDictionary.Add(Entry.AttackType, Entry);
        }
        CreateBossNavAgent();
    }

    private void Update() //State Updates Every 'Update Delay' Time When not Attacking Unless Stunned
    {
        if (isStunned) { return; }

        if (Time.time > nextUpdateCheck)
        {
            nextUpdateCheck = Time.time + UpdateDelay;
            if (HealthScript.BossDefeated) { return; }

            if (!isAttacking)
            {
                MoveUpdate();
                AttackSelection();
            }
        }
    }

    //MOVING
    protected abstract void MoveUpdate();       //Override logic to move around (toward player)


    //ATTACKING
    protected virtual void AttackSelection()
    {
        float PlayerDistance = GetDistanceToPlayer();
        float TotalChance = 0;

        //Accumulate Chance from BossAttack List for Use in Random Selection of Attack
        foreach (BossAttackOption AttackOption in BossAttacks)
        {
            if (!AttackOption.OnCooldown) //Checks Attack is Off Cooldown
            {
                if (AttackOption.DistanceIsMinimum) //Attack is Triggered at Range Above Given Distance
                {
                    if (PlayerDistance >= AttackOption.AttackRangeTrigger)
                    {
                        TotalChance += AttackOption.SelectionChance;
                    }
                }
                else if (!AttackOption.DistanceIsMinimum) //Attack is Triggered at Range Below Given Distance
                {
                    if (PlayerDistance < AttackOption.AttackRangeTrigger)
                    {
                        TotalChance += AttackOption.SelectionChance;
                    }
                }  
            }
        }

        //Get Attack from List, Start the Attack, then Stop Updating/Checking Boss State until Complete
        BossAttackType SelectedAttack = SelectRandomAttack(TotalChance, PlayerDistance);
        if (SelectedAttack == BossAttackType.None) { return; }
        else
        {
            BossAttackOption Attack = AttackDictionary[SelectedAttack];
            Attack.CallAttack(); if (AttackingDuration == null) { NowAttacking(Attack, Attack.Duration); };
        }
    }
    protected void NowAttacking(BossAttackOption AttackOption, float Duration) //Begins isAttacking State for Specific attack's duration
    {
        StartCoroutine(CooldownRoutine(AttackOption, Duration)); 
    }   
    protected IEnumerator AttackingDurationCoroutine(float Duration) //IsAttacking for Duration amount of Time
    {
        isAttacking = true;
        yield return new WaitForSeconds(Duration);
        isAttacking = false;
        AttackingDuration = null;
    }
    protected IEnumerator CooldownRoutine(BossAttackOption AttackOption, float Duration) //Begin and Wait for Duration, then Begin Cooldown Count
    {
        AttackingDuration = StartCoroutine(AttackingDurationCoroutine(Duration));
        yield return AttackingDuration;

        AttackOption.OnCooldown = true;
        yield return new WaitForSeconds(AttackOption.AttackCooldown);
        AttackOption.OnCooldown = false;
    }
    public void InterruptFromStunned(float StunDuration) //Stop Attack and Switch to Stun Timer
    {
        if (AttackingDuration != null)
        {
            StopCoroutine(AttackingDuration);
            AttackingDuration = null;
            isAttacking = false;
        }
        if (StunnedRoutine == null)
        {
            StunnedRoutine = StartCoroutine(BossStunned(StunDuration));
            BossStunExtras();
        }
    }
    protected virtual void BossStunExtras() { } //Extra hook for animations, vfx, sounds, etc
    private IEnumerator BossStunned(float Duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(Duration);
        isStunned = false;
        StopStunExtras();
        StunnedRoutine = null;
    }
    protected virtual void StopStunExtras() { } //Extra hook for animations, vfx, sounds, ect
    private BossAttackType SelectRandomAttack(float TotalChance, float DistanceToPlayer)
    {
        if (TotalChance <= 0) { return BossAttackType.None; }  //Return None if No Attacks Were Available

        float RandomAttack = Random.Range(0, TotalChance);
        float counter = 0f;

        //Search for and return which attack was selected by random chance
        foreach (BossAttackOption AttackOption in BossAttacks)
        {
            if (AttackOption.DistanceIsMinimum) //Attack is Triggered at Range Above Given Distance
            {
                if (DistanceToPlayer >= AttackOption.AttackRangeTrigger)
                {
                    counter += AttackOption.SelectionChance;
                    if (RandomAttack <= counter)
                    {
                        //Return name of Attack Selection so it can be used
                        return AttackOption.AttackType;
                    }
                }
            }
            else if (!AttackOption.DistanceIsMinimum) //Attack is Triggered at Range Below Given Distance
            {
                if (DistanceToPlayer < AttackOption.AttackRangeTrigger)
                {
                    counter += AttackOption.SelectionChance;
                    if (RandomAttack <= counter)
                    {
                        //Return name of Attack Selection so it can be used
                        return AttackOption.AttackType;
                    }
                }
            }
            
        }
        return BossAttackType.None; //Return 'None' if no Attack can be Made
    }
       
    


    protected virtual void CreateBossNavAgent()
    {
        if (GetComponent<NavMeshAgent>() == null)
        {
            BossAgent = gameObject.AddComponent<NavMeshAgent>();
        }
        BossAgent = GetComponent<NavMeshAgent>();
        BossAgent.agentTypeID = 0;
        BossAgent.updateRotation = false;
        BossAgent.updateUpAxis = false;
        BossAgent.speed = MoveSpeed;
        BossAgent.acceleration = 999;
    }
    protected float GetDistanceToPlayer() { return Vector2.Distance(gameObject.transform.position, playerLocation.position); }


    //BASE VISUALS
    protected bool ShouldFaceRight()
    {
        if (BossAgent.destination.x >= gameObject.transform.position.x)
        {
            return true;
        }
        return false;
    }

}
