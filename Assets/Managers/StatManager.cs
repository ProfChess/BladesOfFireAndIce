using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [Header("Allocated Points")]
    [SerializeField] private int Vitality = 1;
    [SerializeField] private int Endurance = 1;
    [SerializeField] private int Strength = 1;
    [SerializeField] private int Dexterity = 1;
    [SerializeField] private int Luck = 1;

    [Header("Minimum and Maximum Points")]
    [Tooltip("Stats Cannot go Higher Than This")]
    [SerializeField] private int MaxPointsPerStat = 10;
    [SerializeField] private int MinPointsPerStat = 1;

    [Header("Total Points to Spend")]
    [SerializeField] private int PointsSpent = 0;
    private const int SpendablePoints = 35;

    //Getting 
    public int VitalityPoints => Vitality;
    public int EndurancePoints => Endurance;
    public int StrengthPoints => Strength;
    public int DexterityPoints => Dexterity;
    public int LuckPoints => Luck;

    public int AvailablePoints => SpendablePoints - PointsSpent;


    //Total Points
    public void CalculateSpentPoints()
    {
        PointsSpent = VitalityPoints +
                      EndurancePoints +
                      StrengthPoints +
                      DexterityPoints +
                      LuckPoints;
    }
    private void StatPointSpend(ref int stat)
    {
        if (stat < MaxPointsPerStat && PointsSpent < SpendablePoints)
        {
            stat += 1;
            PointsSpent++;
        }
    }
    private void StatPointRefund(ref int stat)
    {
        if (stat > MinPointsPerStat && PointsSpent >= 1)
        {
            stat -= 1;
            PointsSpent--;
        }
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
    public void ChangeStrength(int Num) {
        if (Num > 0) { StatPointSpend(ref Strength); }
        else if (Num < 0) { StatPointRefund(ref Strength); }
    }
    public void ChangeDexterity(int Num) {
        if (Num > 0) { StatPointSpend(ref Dexterity); }
        else if (Num < 0) { StatPointRefund(ref Dexterity); }
    }
    public void ChangeLuck(int Num) {
        if (Num > 0) { StatPointSpend(ref Luck); }
        else if (Num < 0) { StatPointRefund(ref Luck); }
    }


}
