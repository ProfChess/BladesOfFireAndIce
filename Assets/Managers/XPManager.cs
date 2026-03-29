using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    //Formula For Stat Points
    //XP Required = BaseValue + (CurrnetPoints^2 x ScaleValue)
    private const int BaseXPNeeded = 150;    //Base Value Needed for Every Stat Point
    private const int ScaleXP = 12;          //Scales Later Points to Be More Expensive
    private int TotalXP = 0;                 //XP Gained Towards Next Level
    private int PlayerLevel = 0;             //Number of Points --> Increased Cost of Higher Stat Points

    public void AddXP(int amount)
    {
        TotalXP += amount;
        RecalculateLevel();
    }
    private void RecalculateLevel()
    {
        int level = 0;
        int xpForNext = BaseXPNeeded + (level + 1) * (level + 1) * ScaleXP;
        int remainingXP = TotalXP;

        while (remainingXP >= xpForNext)
        {
            remainingXP -= xpForNext;
            level++;
            xpForNext = BaseXPNeeded + (level + 1) * (level + 1) * ScaleXP;
        }
        PlayerLevel = level;
    }
    public int StoreSavedXP()
    {
        return TotalXP;
    }
    public int CalculateTotalPointsFromXP(int xp)
    {
        int level = 0;
        int remainingXP = xp;

        while (true)
        {
            int xpNeeded = BaseXPNeeded + (level + 1) * (level + 1) * ScaleXP;
            if (remainingXP < xpNeeded) { break; }

            remainingXP -= xpNeeded;
            level++;
        }

        return level;
    }
    
}
