using System.Collections.Generic;
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
    public int AvailablePoints => SpendablePoints - PointsSpent;

    [Header("Minimum and Maximum Points")]
    [Tooltip("Stats Cannot go Higher Than This")]
    [SerializeField] private int MaxPointsPerStat = 10;
    [SerializeField] private int MinPointsPerStat = 1;
    public int MaxStatPoints => MaxPointsPerStat;
    public int MinStatPoints => MinPointsPerStat;

    [Header("Total Points to Spend")]
    [SerializeField] private int PointsSpent = 5;
    private const int SpendablePoints = 32;

    public void StatPointSpend(ref int stat)
    {
        if (stat < MaxPointsPerStat && PointsSpent < SpendablePoints)
        {
            stat++;
            PointsSpent++;
        }
        SetStats(Vitality, Endurance, Strength, Dexterity, Luck);
    }
    public void StatPointRefund(ref int stat)
    {
        if (stat > MinPointsPerStat && PointsSpent >= 1)
        {
            stat--;
            PointsSpent--;
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



    //Blessings
    [Header("Blessings")]
    public StatBlessing[] VitalityBlessings = new StatBlessing[2];
    public StatBlessing[] EnduranceBlessings = new StatBlessing[2];
    public StatBlessing[] StrengthBlessings = new StatBlessing[2];
    public StatBlessing[] DexterityBlessings = new StatBlessing[2];
    public StatBlessing[] LuckBlessings = new StatBlessing[2];



}
