using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boons/StatBoostBoon")]
public class NumberBoostBoon : BaseBoon
{
    public StatType Stat;
    public float BoonValue;
    public override void BoonSelected()
    {
        PlayerBoonManager.Instance.ChangeBonus(Stat, BoonValue);
    }
    public override void BoonRemoved()
    {
        PlayerBoonManager.Instance.UndoChange(Stat, BoonValue);
    }
}

