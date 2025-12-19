using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Boons/StatBoostBoon")]
public class NumberBoostBoon : BaseBoon
{
    public StatType Stat;
    public int BoonValueLevel1 = 0;
    public int BoonValueLevel2 = 0;
    protected int appliedValue = 0;
    public override void BoonSelected() //Only Called When Boon is New
    {
        if(Level == 1) 
        { 
            PlayerBoonManager.Instance.ChangeBonus(Stat, BoonValueLevel1); 
        }
        else if (Level == 2) 
        { 
            PlayerBoonManager.Instance.ChangeBonus(Stat, BoonValueLevel2);
        }
    }
    public override void BoonRemoved()
    {
        if (Level == 1)
        {
            PlayerBoonManager.Instance.UndoChange(Stat, BoonValueLevel1);
        }
        else if (Level == 2)
        {
            PlayerBoonManager.Instance.UndoChange(Stat, BoonValueLevel2);
        }
    }
    public void IncreaseBonus()
    {
        float value = BoonValueLevel2 - BoonValueLevel1;
        PlayerBoonManager.Instance.ChangeBonus(Stat, value);
    }
    //private void ReApply()
    //{
    //    if (Level == 1)
    //    {
    //        PlayerBoonManager.Instance.ChangeBonus(Stat, BoonValueLevel1);
    //    }
    //    else if (Level == 2)
    //    {
    //        PlayerBoonManager.Instance.ChangeBonus(Stat, BoonValueLevel2);
    //    }
    //}
}

