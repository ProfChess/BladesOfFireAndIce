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

    public void SetStats(int Vit, int Endur, int Str, int Dex, int Luc)
    {
        Vitality = Vit;
        Endurance = Endur;
        Strength = Str;
        Dexterity = Dex;
        Luck = Luc;
    }
}
