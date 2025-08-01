using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    [Header("Allocated Points")]
    //private float MaxPointsPerStat = 10f;
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

    
    //Setting
    public void ChangeVitality(int Num) { Vitality += Num; }
    public void ChangeEndurance(int Num) { Endurance += Num; }
    public void ChangeStrength(int Num) { Strength += Num; }
    public void ChangeDexterity(int Num) { Dexterity += Num; }
    public void ChangeLuck(int Num) { Luck += Num; }


}
