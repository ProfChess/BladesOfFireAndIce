using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatAllocationCenter : InteractableObject
{
    [SerializeField] private GameObject StatMenu;
    public override void Interact()
    {
        base.Interact();
        StatMenu.SetActive(true);
    }

    //Formula For Stat Points
    //XP Required = BaseValue + (CurrnetPoints^2 x ScaleValue)
    private const int BaseXPNeeded = 150;    //Base Value Needed for Every Stat Point
    private int CurrentTotalStatPoints = 0;  //Number of Points --> Increased Cost of Higher Stat Points
    private const int ScaleXP = 12;          //Scales Later Points to Be More Expensive
    private int SavedXP = 0;                 //XP Gained Towards Next Level
    private int CalcXPNeeded => BaseXPNeeded + (CurrentTotalStatPoints + 1) * (CurrentTotalStatPoints + 1) * ScaleXP;

    public void SaveRunXP(int GainedXP)
    {
        SavedXP += GainedXP;
        int XPNeeded = CalcXPNeeded;
        if (SavedXP >= XPNeeded)
        {
            //Gain Stat Point
            CurrentTotalStatPoints++;

            //Set SavedXP to Any Left Over XP Gained From Previous Run
            int overlevelXP = SavedXP - XPNeeded;
            SavedXP = overlevelXP;
        }
    }
    
}
