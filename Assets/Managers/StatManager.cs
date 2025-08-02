using System.Collections;
using System.Collections.Generic;
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


    //Getting 
    public int VitalityPoints => Vitality;
    public int EndurancePoints => Endurance;
    public int StrengthPoints => Strength;
    public int DexterityPoints => Dexterity;
    public int LuckPoints => Luck;

    
    //Setting
    public void ChangeVitality(int Num) 
    { 
        Vitality += Num; 
        Vitality = Vitality > MaxPointsPerStat? MaxPointsPerStat : Vitality;
        Vitality = Vitality < MinPointsPerStat ? MinPointsPerStat : Vitality;
    }
    public void ChangeEndurance(int Num) 
    { 
        Endurance += Num;
        Endurance = Endurance > MaxPointsPerStat ? MaxPointsPerStat : Endurance;
        Endurance = Endurance < MinPointsPerStat ? MinPointsPerStat : Endurance;
    }
    public void ChangeStrength(int Num) { 
        Strength += Num;
        Strength = Strength > MaxPointsPerStat ? MaxPointsPerStat : Strength;
        Strength = Strength < MinPointsPerStat ? MinPointsPerStat : Strength;
    }
    public void ChangeDexterity(int Num) { 
        Dexterity += Num;
        Dexterity = Dexterity > MaxPointsPerStat ? MaxPointsPerStat : Dexterity;
        Dexterity = Dexterity < MinPointsPerStat ? MinPointsPerStat : Dexterity;
    }
    public void ChangeLuck(int Num) {
        Luck += Num;
        Luck = Luck > MaxPointsPerStat ? MaxPointsPerStat : Luck;
        Luck = Luck < MinPointsPerStat ? MinPointsPerStat : Luck;
    }


}
