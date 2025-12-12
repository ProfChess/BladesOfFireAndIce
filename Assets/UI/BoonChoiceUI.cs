using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoonChoiceUI : MonoBehaviour
{
    //Current Boon To Display
    private BaseBoon displayedBoon;

    //Fields for Visual Elements
    [SerializeField] private Text TitleText;
    [SerializeField] private Text DescriptionText;
    //[SerializeField] private Image iconImage;

    public void AssignBoonVisuals(BaseBoon boon)
    {
        displayedBoon = boon;
        TitleText.text = boon.boonName;
        DescriptionText.text = boon.boonDescription;
        //iconImage.sprite = boon.Icon;
    }

    public void OnChoose()
    {
        GameManager.Instance.boonOptions.BoonChoiceMade(displayedBoon);
    }
}
