using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBuff_Vitality : BaseTempBuff
{
    [SerializeField] private int DamageResistance = 0;
    public override void ApplyBuff(PlayerStatSetting Stats)
    {
        Stats.ApplyBonusStat(StatType.Armor, DamageResistance);
        Debug.Log("Buff Applied");
    }

    public override void DeactivateBuff(PlayerStatSetting Stats)
    {
        Stats.ApplyBonusStat(StatType.Armor, -DamageResistance);
        Debug.Log("Buff Removed");
    }
}
