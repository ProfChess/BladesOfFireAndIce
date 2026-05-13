using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlessingChoiceUI : MonoBehaviour
{
    private StatBlessing Blessing;
    [SerializeField] private TextMeshProUGUI Name;
    [SerializeField] private TextMeshProUGUI Desc;
    [SerializeField] private Toggle BlessingToggle;
    [SerializeField] private TextMoveToggle BlessingToggleText;

    public void AssignInfo(StatBlessing blessing)
    {
        Blessing = blessing;
        Name.text = blessing.BonusName;
        Desc.text = blessing.BonusDescription;
        BlessingToggle.SetIsOnWithoutNotify(GameManager.Instance.runData.IsBlessingSelected(blessing));
        BlessingToggleText.Refresh();
    }

    public void OnSelectBlessing()
    {
        if (Blessing == null) { return; }
        GameManager.Instance.runData.ToggleBlessing(Blessing);
    }
}
