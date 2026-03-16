using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlessingChoiceUI : MonoBehaviour
{
    private StatBlessing Blessing;
    [SerializeField] private Text Name;
    [SerializeField] private Text Desc;
    [SerializeField] private Toggle BlessingToggle;
    [SerializeField] private TextMoveToggle BlessingToggleText;

    public void AssignInfo(StatBlessing blessing)
    {
        Blessing = blessing;
        Name.text = blessing.BlessingName;
        Desc.text = blessing.BlessingDescription;
        BlessingToggle.SetIsOnWithoutNotify(GameManager.Instance.runData.IsBlessingSelected(blessing));
        BlessingToggleText.Refresh();
    }

    public void OnSelectBlessing()
    {
        if (Blessing == null) { return; }
        GameManager.Instance.runData.ToggleBlessing(Blessing);
    }
}
