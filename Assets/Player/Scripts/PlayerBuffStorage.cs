using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffStorage : MonoBehaviour
{
    [SerializeField] private PlayerStatSetting playerStats;

    //Dictionary for Tracking Buffs, float = remaining time
    private Dictionary<TempBuffType, float> BuffTimers = new();
    private Dictionary<TempBuffType, BaseTempBuff> activeBuffs = new();
    private List<TempBuffType> buffKeys = new();

    private float nextCheck = 0f;
    private float checkInterval = 0.1f;
    public void AddBuff(BaseTempBuff Buff)
    {
        //Log the Buff Duration
        if (activeBuffs.ContainsKey(Buff.BuffType)) { BuffTimers[Buff.BuffType] = Buff.BuffDuration; }
        else
        {
            activeBuffs[Buff.BuffType] = Buff;
            BuffTimers[Buff.BuffType] = Buff.BuffDuration;
            Buff.ApplyBuff(playerStats);
        }
    }
    private void Update()
    {
        if (Time.time >= nextCheck)
        {
            //Set Next Check
            nextCheck = Time.time + checkInterval;

            //Clear and Reset Buff List to Check
            buffKeys.Clear();
            buffKeys.AddRange(BuffTimers.Keys);

            //Decrease all Cooldowns
            foreach(TempBuffType BuffType in buffKeys)
            {
                BuffTimers[BuffType] -= checkInterval;
                if (BuffTimers[BuffType] <= 0f)
                {
                    BuffTimers.Remove(BuffType);
                    activeBuffs[BuffType].DeactivateBuff(playerStats);
                    activeBuffs.Remove(BuffType);
                }
            }
        }
    }
}
