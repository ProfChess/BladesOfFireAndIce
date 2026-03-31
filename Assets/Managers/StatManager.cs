using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [Header("Saved Points")]
    [SerializeField] private int Vitality = 1;
    [SerializeField] private int Endurance = 1;
    [SerializeField] private int Strength = 1;
    [SerializeField] private int Dexterity = 1;
    [SerializeField] private int Luck = 1;

    //Getting 
    public int VitalityPoints => Vitality;
    public int EndurancePoints => Endurance;
    public int StrengthPoints => Strength;
    public int DexterityPoints => Dexterity;
    public int LuckPoints => Luck;
    public int AvailablePoints => PointsAvailable;

    [Header("Minimum and Maximum Points")]
    [Tooltip("Stats Cannot go Higher Than This")]
    [SerializeField] private int MaxPointsPerStat = 10;
    [SerializeField] private int MinPointsPerStat = 1;
    public int MaxStatPoints => MaxPointsPerStat;
    public int MinStatPoints => MinPointsPerStat;

    [Header("Total Points to Spend")]
    [SerializeField] private int PointsSpent = 0;
    private int PointsAvailable = 26;
    private const int MaxTotalPoints = 26;          //Will be used to limit attained points

    public void StatPointSpend(ref int stat)
    {
        if (stat < MaxPointsPerStat && PointsAvailable >= 1 && PointsSpent <= MaxTotalPoints)
        {
            stat++;
            PointsSpent++;
            PointsAvailable--;
        }
        SetStats(Vitality, Endurance, Strength, Dexterity, Luck);
    }
    public void StatPointRefund(ref int stat)
    {
        if (stat > MinPointsPerStat && PointsSpent >= 1)
        {
            stat--;
            PointsSpent--;
            PointsAvailable++;
        }
        SetStats(Vitality, Endurance, Strength, Dexterity, Luck);
    }
    public void CalculateSpentPoints()
    {
        PointsSpent = VitalityPoints +
                      EndurancePoints +
                      StrengthPoints +
                      DexterityPoints +
                      LuckPoints;
    }
    private void SetStats(int Vit, int Endur, int Str, int Dex, int Luc)
    {
        Vitality = Vit;
        Endurance = Endur;
        Strength = Str;
        Dexterity = Dex;
        Luck = Luc;
    }

    //Setting
    public void ChangeVitality(int Num)
    {
        if (Num > 0) { StatPointSpend(ref Vitality); }
        else if (Num < 0) { StatPointRefund(ref Vitality); }
    }
    public void ChangeEndurance(int Num)
    {
        if (Num > 0) { StatPointSpend(ref Endurance); }
        else if (Num < 0) { StatPointRefund(ref Endurance); }
    }
    public void ChangeStrength(int Num)
    {
        if (Num > 0) { StatPointSpend(ref Strength); }
        else if (Num < 0) { StatPointRefund(ref Strength); }
    }
    public void ChangeDexterity(int Num)
    {
        if (Num > 0) { StatPointSpend(ref Dexterity); }
        else if (Num < 0) { StatPointRefund(ref Dexterity); }
    }
    public void ChangeLuck(int Num)
    {
        if (Num > 0) { StatPointSpend(ref Luck); }
        else if (Num < 0) { StatPointRefund(ref Luck); }
    }


    //Save and Load
    public void SetPointsAvailable(int num) { PointsAvailable = num; CalculateSpentPoints(); }
    public int GetPointsAvailable() { return PointsAvailable; }
    public List<Save_StatValue> GetStatPoints()
    {
        List<Save_StatValue> StatNumbers = new();

        foreach(SaveStats statType in Enum.GetValues(typeof(SaveStats)))
        {
            int value = GetStatPointsFromStat(statType);
            StatNumbers.Add(new Save_StatValue() { stat = statType, value = value });
        }

        return StatNumbers;
    }
    //Returns Number of Invested Points of a Given Stat
    private int GetStatPointsFromStat(SaveStats stat)
    {
        switch (stat)
        {
            case SaveStats.Vitality: return VitalityPoints;
            case SaveStats.Endurance: return EndurancePoints;
            case SaveStats.Strength: return StrengthPoints;
            case SaveStats.Dexterity: return DexterityPoints;
            case SaveStats.Luck: return LuckPoints;
        }
        return 1;
    }
    public void AssignStatPointsFromSave(List<Save_StatValue> statNumbers)
    {
        foreach (SaveStats statType in Enum.GetValues(typeof(SaveStats)))
        {
            int value = statNumbers.FirstOrDefault(s => s.stat == statType)?.value ?? 1;

            switch (statType)
            {
                case SaveStats.Vitality: Vitality = value; break;
                case SaveStats.Endurance: Endurance = value; break;
                case SaveStats.Strength: Strength = value; break;
                case SaveStats.Dexterity: Dexterity = value; break;
                case SaveStats.Luck: Luck = value; break;
            }
        }

    }

    //Blessings
    [Header("Blessings")]
    public StatBlessing[] VitalityBlessings = new StatBlessing[2];
    public StatBlessing[] EnduranceBlessings = new StatBlessing[2];
    public StatBlessing[] StrengthBlessings = new StatBlessing[2];
    public StatBlessing[] DexterityBlessings = new StatBlessing[2];
    public StatBlessing[] LuckBlessings = new StatBlessing[2];

    //Returns all selected blessings for saving the choices
    public List<Save_StatBonusStorage> GetSelectedBlessings()
    {
        List<Save_StatBonusStorage> BlessingsSelected = new List<Save_StatBonusStorage>();
        foreach (SaveStats statType in Enum.GetValues(typeof(SaveStats)))
        {
            int Choice = -1;
            StatBlessing[] CurrentBlessingCollection = GetBlessingCollectionFromStat(statType);
            for (int i = 0; i < CurrentBlessingCollection.Length; i++) 
            {
                if (GameManager.Instance.runData.IsBlessingSelected(CurrentBlessingCollection[i])) 
                { 
                    Choice = i; break; 
                }
            }
            BlessingsSelected.Add(new Save_StatBonusStorage() { stat = statType, chosenOption = Choice});
        }
        return BlessingsSelected;
    }
    public void AssignSelectedBlessings(List<Save_StatBonusStorage> BlessingsSelected)
    {
        //Loop Through all Saved Blessing Choices
        foreach (Save_StatBonusStorage BlessingChoice in BlessingsSelected)
        {
            //Skip if no Blessing is Selected
            if (BlessingChoice.chosenOption == -1) { continue; }
            
            //Get Current Blessing
            StatBlessing Blessing = GetBlessingCollectionFromStat(BlessingChoice.stat)[BlessingChoice.chosenOption];
            
            //If no Blessing, or if Blessing is already applied, cease
            if (Blessing == null || GameManager.Instance.runData.IsBlessingSelected(Blessing)) { continue; }

            //Apply Blessing
            GameManager.Instance.runData.SelectBlessing(Blessing);
        }
    }
    private StatBlessing[] GetBlessingCollectionFromStat(SaveStats stat)
    {
        switch (stat)
        {
            case SaveStats.Vitality: return VitalityBlessings;
            case SaveStats.Endurance: return EnduranceBlessings;
            case SaveStats.Strength: return StrengthBlessings;
            case SaveStats.Dexterity: return DexterityBlessings;
            case SaveStats.Luck: return LuckBlessings;
        }
        Debug.Log("Stat not Found");
        return null;
    }

}
