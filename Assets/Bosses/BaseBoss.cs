using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[System.Serializable]
public class BossAttackOption
{
    [Tooltip("Name of Attack")]
    public BossAttackType AttackType;
    [Tooltip("Chance Attack is Selected When Within Range")]
    public float SelectionChance;
    [Tooltip("Cooldown of Attack")]
    public float AttackCooldown;
    public bool OnCooldown = false;
    [Tooltip("Distance to Player to Trigger Attack")]
    public float AttackRangeTrigger;
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
    None, AOE, DoubleBlast, ChargeAttack
}
public abstract class BaseBoss : BaseHealth
{
    //State Updates
    private float UpdateDelay = 0.2f;
    private float nextUpdateCheck = 0f;

    //Movement
    [SerializeField] protected float MoveSpeed;
    [SerializeField] protected float attackCooldown;
    protected NavMeshAgent BossAgent;

    //Attacking
    protected bool isAttacking = false;
    protected float attackTimer;
    protected Transform playerLocation;

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

    protected void Start()
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
            //Makes sure Within Range and Off Cooldown
            if (PlayerDistance <= AttackOption.AttackRangeTrigger && !AttackOption.OnCooldown)
            {
                TotalChance += AttackOption.SelectionChance;
            }
        }

        //Get Attack from List, Start the Attack, then Stop Updating/Checking Boss State until Complete
        BossAttackType SelectedAttack = SelectRandomAttack(TotalChance, PlayerDistance);
        if (SelectedAttack == BossAttackType.None) { return; }
        else
        {
            BossAttackOption Attack = AttackDictionary[SelectedAttack];
            Attack.CallAttack(); NowAttacking();
        }
    }
    protected void NowAttacking() { isAttacking = true; }   
    public void NotAttacking() { isAttacking = false; }

    private BossAttackType SelectRandomAttack(float TotalChance, float DistanceToPlayer)
    {
        if (TotalChance <= 0) { return BossAttackType.None; }  //Return None if No Attacks Were Available

        float RandomAttack = Random.Range(0, TotalChance);
        float counter = 0f;

        //Search for and return which attack was selected by random chance
        foreach (BossAttackOption AttackOption in BossAttacks)
        {
            if (DistanceToPlayer <= AttackOption.AttackRangeTrigger)
            {
                counter += AttackOption.SelectionChance;
                if (RandomAttack <= counter)
                {
                    //Return name of Attack Selection so it can be used
                    return AttackOption.AttackType;
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
