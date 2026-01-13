using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuffStorage : MonoBehaviour
{
    [SerializeField] private PlayerStatSetting playerStats;

    //Dictionary for Tracking Buffs, float = remaining time
    private Dictionary<TempBuffType, float> BuffTimers = new();
    private Dictionary<TempBuffType, BaseTempBuff> activeBuffs = new();
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
}
