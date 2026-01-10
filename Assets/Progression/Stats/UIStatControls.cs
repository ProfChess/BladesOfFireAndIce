using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatControls : MonoBehaviour
{
    [Header("Stat Allocation Center")]
    [SerializeField] private StatAllocationCenter StatAllocation;

    [Header("Stat Texts")]
    [SerializeField] private Text vitalityNum;
    [SerializeField] private Text enduranceNum;
    [SerializeField] private Text strengthNum;
    [SerializeField] private Text dexterityNum;
    [SerializeField] private Text luckNum;

    [Header("Points Text")]
    [SerializeField] private Text AvailablePoints;

    private void UpdateStatVisuals()
    {
        vitalityNum.text = StatAllocation.VitalityPoints.ToString();
        enduranceNum.text = StatAllocation.EndurancePoints.ToString();
        strengthNum.text = StatAllocation.StrengthPoints.ToString();
        dexterityNum.text = StatAllocation.DexterityPoints.ToString();
        luckNum.text = StatAllocation.LuckPoints.ToString();
        AvailablePoints.text = StatAllocation.AvailablePoints.ToString();
    }
    private void Start()
    {
        StatAllocation.CalculateSpentPoints();
        UpdateStatVisuals();
    }


    //Vitality
    public void PlusVitality() { StatAllocation.ChangeVitality(1); UpdateStatVisuals(); }
    public void MinusVitality() { StatAllocation.ChangeVitality(-1); UpdateStatVisuals(); }

    //Endurance
    public void PlusEndurance() { StatAllocation.ChangeEndurance(1); UpdateStatVisuals(); }
    public void MinusEndurance() { StatAllocation.ChangeEndurance(-1); UpdateStatVisuals(); }

    //Strength
    public void PlusStrength() { StatAllocation.ChangeStrength(1); UpdateStatVisuals(); }
    public void MinusStrength() { StatAllocation.ChangeStrength(-1); UpdateStatVisuals(); }

    //Dexterity
    public void PlusDexterity() { StatAllocation.ChangeDexterity(1); UpdateStatVisuals(); }
    public void MinusDexterity() { StatAllocation.ChangeDexterity(-1); UpdateStatVisuals(); }

    //Luck
    public void PlusLuck() { StatAllocation.ChangeLuck(1); UpdateStatVisuals(); }
    public void MinusLuck() { StatAllocation.ChangeLuck(-1); UpdateStatVisuals(); }
}
