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
    [SerializeField] private Text strengthNum;
    [SerializeField] private Text dexterityNum;
    [SerializeField] private Text luckNum;

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
    }


    //Vitality
    public void VitalityPlus() { SM.ChangeVitality(1); UpdateStatVisuals(); CheckStat(SM.VitalityPoints, vitalityBlessingButton); }
    public void VitalityMinus() { SM.ChangeVitality(-1); UpdateStatVisuals(); CheckStat(SM.VitalityPoints, vitalityBlessingButton); }
    public void VitalityBlessingMenu()
    {
        FirstBlessingOption.AssignInfo(SM.VitalityBlessings[0]);
        SecondBlessingOption.AssignInfo(SM.VitalityBlessings[1]);
        BlessingSelectMenu.SetActive(true);
        GameManager.Instance.MenuOpened(BlessingSelectMenu);
    }

    //Endurance
    public void EndurancePlus() { SM.ChangeEndurance(1); UpdateStatVisuals(); }
    public void EnduranceMinus() { SM.ChangeEndurance(-1); UpdateStatVisuals(); }

    //Strength
    public void StrengthPlus() { SM.ChangeStrength(1); UpdateStatVisuals(); }
    public void StrengthMinus() { SM.ChangeStrength(-1); UpdateStatVisuals(); }

    //Dexterity
    public void DexterityPlus() { SM.ChangeDexterity(1); UpdateStatVisuals(); }
    public void DexterityMinus() { SM.ChangeDexterity(-1); UpdateStatVisuals(); }

    //Luck
    public void LuckPlus() { SM.ChangeLuck(1); UpdateStatVisuals(); }
    public void LuckMinus() { SM.ChangeLuck(-1); UpdateStatVisuals(); }

    //Blessings
    public void OpenCloseBlessingMenu(bool isOpen)
    {
        BlessingSelectMenu.SetActive(isOpen);
    }
    private void CheckStat(float statCurrentPoints, Button buttonToConsider)
    {
        buttonToConsider.interactable = statCurrentPoints == SM.MaxStatPoints;
    }
    
}
