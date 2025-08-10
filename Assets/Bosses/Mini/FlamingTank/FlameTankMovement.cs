using System;
using System.Collections.Generic;
using UnityEngine;

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
public class FlameTankMovement : BaseBoss
{
    //List
    [Header("List of Boss Attacks")]
    [SerializeField] private List<BossAttackOption> BossAttacks = new List<BossAttackOption>();
    private Dictionary<BossAttackType, BossAttackOption> AttackDictionary = 
        new Dictionary<BossAttackType, BossAttackOption>();

    private void Start()
    {
        foreach (BossAttackOption Entry in BossAttacks) //Create Dictionary of Attacks for Easy Acces
        {
            AttackDictionary.Add(Entry.AttackType, Entry);
        }
    }
    protected override void AttackSelection()
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
            BossAttackOption Attack =  AttackDictionary[SelectedAttack]; 
            Attack.CallAttack(); NowAttacking(); 
        }
    }
    private BossAttackType SelectRandomAttack(float TotalChance, float DistanceToPlayer)
    {
        if (TotalChance <= 0) {  return BossAttackType.None; }  //Return None if No Attacks Were Available

        float RandomAttack = UnityEngine.Random.Range(0, TotalChance);
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

    //Movement
    protected override void MoveUpdate()
    {
        BossAgent.SetDestination(playerLocation.position);
    }

}


