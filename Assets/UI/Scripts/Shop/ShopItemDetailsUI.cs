using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemDetailsUI : BaseStatUIDisplay
{
    [Header("Details")]
    [SerializeField] private GameObject InfoElementsParent;
    [SerializeField] private TextMeshProUGUI ItemNameText;
    [SerializeField] private TextMeshProUGUI ItemDescText;
    [SerializeField] private GameObject LevelLabel;
    [SerializeField] private TextMeshProUGUI LevelNumText;
    [SerializeField] private TextMeshProUGUI TypeText;

    public void AssignItemDetails(string itemName, string itemDesc, BonusType type, List<StatDisplayEntry> statList, int level = 0)
    {
        //Item Basic Info
        InfoElementsParent.SetActive(true);
        ItemNameText.text = itemName; ItemDescText.text = itemDesc; TypeText.text = type.ToString();
        if (level == 0) { LevelLabel.SetActive(false); }
        LevelNumText.text = level.ToString();

        //Input Stats
        AssignStatsFromItem(statList);
    }
    public void ClearSelection()
    {
        InfoElementsParent.SetActive(false);
    }

}
