using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingChoiceUI : MonoBehaviour
{
    private StatBlessing Blessing;
    private string Name = string.Empty;
    [TextArea] private string Desc = string.Empty; 

    public void AssignInfo(StatBlessing blessing)
    {
        Blessing = blessing;
        Name = blessing.BlessingName;
        Desc = blessing.BlessingDescription;
    }

    public void OnSelectBlessing()
    {
        GameManager.Instance.runData.ToggleBlessing(Blessing);
    }
}
