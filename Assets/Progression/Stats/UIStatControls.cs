using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatControls : MonoBehaviour
{
    [Header("Stat Texts")]
    [SerializeField] private Text vitalityNum;
    [SerializeField] private Button vitalityBlessingButton;
    [SerializeField] private Text enduranceNum;
    [SerializeField] private Button enduranceBlessingButton;
    [SerializeField] private Text strengthNum;
    [SerializeField] private Button strengthBlessingButton;
    [SerializeField] private Text dexterityNum;
    [SerializeField] private Button dexterityBlessingButton;
    [SerializeField] private Text luckNum;
    [SerializeField] private Button luckBlessingButton;

    [Header("Points Text")]
    [SerializeField] private Text AvailablePoints;

    private StatManager SM => GameManager.Instance.statManager;

    //Blessings
    [Header("Blessings")]
    [SerializeField] private GameObject BlessingSelectMenu;
    [SerializeField] private BlessingChoiceUI FirstBlessingOption;
    [SerializeField] private BlessingChoiceUI SecondBlessingOption;

    private void OnEnable()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.MenuOpened(gameObject);
        }
    }

    private void UpdateStatVisuals()
    {
        vitalityNum.text = SM.VitalityPoints.ToString();
        enduranceNum.text = SM.EndurancePoints.ToString();
        strengthNum.text = SM.StrengthPoints.ToString();
        dexterityNum.text = SM.DexterityPoints.ToString();
        luckNum.text = SM.LuckPoints.ToString();
        AvailablePoints.text = SM.AvailablePoints.ToString();
    }
    private void Start()
    {
        SM.CalculateSpentPoints();
        UpdateStatVisuals();
        CheckAllStats();
    }

    //Vitality
    public void VitalityPlus() 
    { 
        SM.ChangeVitality(1); UpdateStatVisuals(); 
        CheckStat(SM.VitalityPoints, vitalityBlessingButton, SM.VitalityBlessings); 
    }
    public void VitalityMinus() 
    { 
        SM.ChangeVitality(-1); UpdateStatVisuals(); 
        CheckStat(SM.VitalityPoints, vitalityBlessingButton, SM.VitalityBlessings); 
    }
    public void VitalityBlessingMenu()
    {
        FirstBlessingOption.AssignInfo(SM.VitalityBlessings[0]);
        SecondBlessingOption.AssignInfo(SM.VitalityBlessings[1]);
        BlessingSelectMenu.SetActive(true);
        GameManager.Instance.MenuOpened(BlessingSelectMenu);
    }

    //Endurance
    public void EndurancePlus() 
    { 
        SM.ChangeEndurance(1); UpdateStatVisuals();
        CheckStat(SM.EndurancePoints, enduranceBlessingButton, SM.EnduranceBlessings);
    }
    public void EnduranceMinus() 
    { 
        SM.ChangeEndurance(-1); UpdateStatVisuals();
        CheckStat(SM.EndurancePoints, enduranceBlessingButton, SM.EnduranceBlessings);
    }
    public void EnduranceBlessingMenu()
    {
        FirstBlessingOption.AssignInfo(SM.EnduranceBlessings[0]);
        SecondBlessingOption.AssignInfo(SM.EnduranceBlessings[1]);
        BlessingSelectMenu.SetActive(true);
        GameManager.Instance.MenuOpened(BlessingSelectMenu);
    }

    //Strength
    public void StrengthPlus() 
    { 
        SM.ChangeStrength(1); UpdateStatVisuals();
        CheckStat(SM.StrengthPoints, strengthBlessingButton, SM.StrengthBlessings);
    }
    public void StrengthMinus() 
    { 
        SM.ChangeStrength(-1); UpdateStatVisuals();
        CheckStat(SM.StrengthPoints, strengthBlessingButton, SM.StrengthBlessings);
    }
    public void StrengthBlessingMenu()
    {
        FirstBlessingOption.AssignInfo(SM.StrengthBlessings[0]);
        SecondBlessingOption.AssignInfo(SM.StrengthBlessings[1]);
        BlessingSelectMenu.SetActive(true);
        GameManager.Instance.MenuOpened(BlessingSelectMenu);
    }

    //Dexterity
    public void DexterityPlus() 
    { 
        SM.ChangeDexterity(1); UpdateStatVisuals();
        CheckStat(SM.DexterityPoints, dexterityBlessingButton, SM.DexterityBlessings);
    }
    public void DexterityMinus() 
    { 
        SM.ChangeDexterity(-1); UpdateStatVisuals();
        CheckStat(SM.DexterityPoints, dexterityBlessingButton, SM.DexterityBlessings);
    }
    public void DexterityBlessingMenu()
    {
        FirstBlessingOption.AssignInfo(SM.DexterityBlessings[0]);
        SecondBlessingOption.AssignInfo(SM.DexterityBlessings[1]);
        BlessingSelectMenu.SetActive(true);
        GameManager.Instance.MenuOpened(BlessingSelectMenu);
    }

    //Luck
    public void LuckPlus() 
    { 
        SM.ChangeLuck(1); UpdateStatVisuals();
        CheckStat(SM.LuckPoints, luckBlessingButton, SM.LuckBlessings);
    }
    public void LuckMinus() 
    { 
        SM.ChangeLuck(-1); UpdateStatVisuals();
        CheckStat(SM.LuckPoints, luckBlessingButton, SM.LuckBlessings);
    }
    public void LuckBlessingMenu()
    {
        FirstBlessingOption.AssignInfo(SM.LuckBlessings[0]);
        SecondBlessingOption.AssignInfo(SM.LuckBlessings[1]);
        BlessingSelectMenu.SetActive(true);
        GameManager.Instance.MenuOpened(BlessingSelectMenu);
    }

    //Blessings
    public void OpenCloseBlessingMenu(bool isOpen)
    {
        BlessingSelectMenu.SetActive(isOpen);
    }
    private void CheckStat(float statCurrentPoints, Button buttonToConsider, StatBlessing[] Blessings)
    {
        buttonToConsider.interactable = statCurrentPoints == SM.MaxStatPoints;
        if (statCurrentPoints < SM.MaxStatPoints)
        {
            //Disable Blessing Upon Lowering Max Stats
            GameManager.Instance.runData.DisableAllBlessingsOfType(Blessings);
        }
    }
    private void CheckAllStats()
    {
        CheckStat(SM.VitalityPoints, vitalityBlessingButton, SM.VitalityBlessings);
        CheckStat(SM.EndurancePoints, enduranceBlessingButton, SM.EnduranceBlessings);
        //CheckStat(SM.StrengthPoints, strengthBlessingButton, SM.StrengthBlessings);
        //CheckStat(SM.DexterityPoints, dexterityBlessingButton, SM.DexterityBlessings);
        //CheckStat(SM.LuckPoints, luckBlessingButton, SM.LuckBlessings);
    }

}
