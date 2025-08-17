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



    private void OnEnable()
    {
        GameManager.Instance.getNavMesh().MeshCreated += CreateBossNavAgent;
    }
    private void OnDisable()
    {
        GameManager.Instance.getNavMesh().MeshCreated -= CreateBossNavAgent;
    }

    protected virtual void Start()
    {
        playerLocation = GameManager.Instance.getPlayer().transform;
        CreateBossNavAgent();
        foreach (BossAttackOption Entry in BossAttacks) //Create Dictionary of Attacks for Easy Acces
        {
            AttackDictionary.Add(Entry.AttackType, Entry);
        }
    }

    private void Update()
    {
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
            Attack.CallAttack(); if (AttackingDuration == null) { NowAttacking(Attack.Duration); };
        }
    }
    protected void NowAttacking(float Duration) //Begins isAttacking State for Specific attack's duration
    { 
        AttackingDuration = StartCoroutine(AttackingDurationCoroutine(Duration)); 
    }   
    protected IEnumerator AttackingDurationCoroutine(float Duration)
    {
        isAttacking = true;
        yield return new WaitForSeconds(Duration);
        isAttacking = false;
        AttackingDuration = null;
    }
    public void NotAttacking() { isAttacking = false; }

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
            gameObject.AddComponent<NavMeshAgent>();
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
