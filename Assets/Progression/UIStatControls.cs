using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStatControls : MonoBehaviour
{
    [SerializeField] private Text vitalityNum;
    [SerializeField] private Text enduranceNum;
    [SerializeField] private Text strengthNum;
    [SerializeField] private Text dexterityNum;
    [SerializeField] private Text luckNum;

    private void UpdateStatVisuals()
    {
        vitalityNum.text = SM.VitalityPoints.ToString();
        enduranceNum.text = SM.EndurancePoints.ToString();
        strengthNum.text = SM.StrengthPoints.ToString();
        dexterityNum.text = SM.DexterityPoints.ToString();
        luckNum.text = SM.LuckPoints.ToString();
    }
    private void Start()
    {
        UpdateStatVisuals();
    }


    private StatManager SM => GameManager.Instance.statManager;

    //Vitality
    public void PlusVitality() { SM.ChangeVitality(1); UpdateStatVisuals(); }
    public void MinusVitality() { SM.ChangeVitality(-1); UpdateStatVisuals(); }

    //Endurance
    public void PlusEndurance() { SM.ChangeEndurance(1); UpdateStatVisuals(); }
    public void MinusEndurance() { SM.ChangeEndurance(-1); UpdateStatVisuals(); }

    //Strength
    public void PlusStrength() { SM.ChangeStrength(1); UpdateStatVisuals(); }
    public void MinusStrength() { SM.ChangeStrength(-1); UpdateStatVisuals(); }

    //Dexterity
    public void PlusDexterity() { SM.ChangeDexterity(1); UpdateStatVisuals(); }
    public void MinusDexterity() { SM.ChangeDexterity(-1); UpdateStatVisuals(); }

    //Luck
    public void PlusLuck() { SM.ChangeLuck(1); UpdateStatVisuals(); }
    public void MinusLuck() { SM.ChangeLuck(-1); UpdateStatVisuals(); }
}
