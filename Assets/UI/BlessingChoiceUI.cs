using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlessingChoiceUI : BaseStatUIDisplay
{
    private StatBlessing Blessing;
    [Header("Details")]
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Desc;
    [Header("Other")]
    [SerializeField] private Toggle BlessingToggle;
    [SerializeField] private TextMoveToggle BlessingToggleText;

    public void AssignInfo(StatBlessing blessing)
    {
        //Details
        Blessing = blessing;
        Name.text = blessing.BonusName;
        Desc.text = blessing.BonusDescription;
        //Stats
        AssignStatsFromItem(blessing.GetDisplayListofEffects());

        //Text Refresh
        BlessingToggle.SetIsOnWithoutNotify(GameManager.Instance.runData.IsBlessingSelected(blessing));
        BlessingToggleText.Refresh();
    }

    public void OnSelectBlessing()
    {
        if (Blessing == null) { return; }
        GameManager.Instance.runData.ToggleBlessing(Blessing);
    }
}
