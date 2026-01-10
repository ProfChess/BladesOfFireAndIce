using UnityEngine;

public class StatAllocationCenter : InteractableObject
{
    [SerializeField] private GameObject StatMenu;
    private void OnEnable()
    {
        GameManager.Instance.MenuClosed += CloseMenu;

    }
    private void OnDisable()
    {
        GameManager.Instance.MenuClosed -= CloseMenu;
    }

    private void CloseMenu()
    {
        StatMenu.SetActive(false);
        GameManager.Instance.ChangePlayerToPlayerActions();
    }

    public override void Interact()
    {
        base.Interact();
        StatMenu.SetActive(true);
        GameManager.Instance.ChangePlayerToUIActions();
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

    //Stats
    private int Vitality = 1;
    private int Endurance = 1;
    private int Strength = 1;
    private int Dexterity = 1;
    private int Luck = 1;
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

    [Header("Total Points to Spend")]
    [SerializeField] private int PointsSpent = 5;
    private const int SpendablePoints = 32;

    //Shortcut to Stat Manager
    private StatManager SM => GameManager.Instance.statManager;

    private void StatPointSpend(ref int stat)
    {
        if (stat < MaxPointsPerStat && PointsSpent < SpendablePoints)
        {
            stat++;
            PointsSpent++;
        }
        SM.SetStats(Vitality, Endurance, Strength, Dexterity, Luck);
    }
    private void StatPointRefund(ref int stat)
    {
        if (stat > MinPointsPerStat && PointsSpent >= 1)
        {
            stat--;
            PointsSpent--;
        }
        SM.SetStats(Vitality, Endurance, Strength, Dexterity, Luck);
    }
    public void CalculateSpentPoints()
    {
        PointsSpent = VitalityPoints +
                      EndurancePoints +
                      StrengthPoints +
                      DexterityPoints +
                      LuckPoints;
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

}
