using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipStat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI desc;

    public void DisplayInformation(StatDisplayInfo info)
    {
        title.text = info.Name;
        desc.text = info.Description;
    }
}
