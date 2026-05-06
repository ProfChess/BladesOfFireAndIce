using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemStatAccess : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StatName;
    [SerializeField] private TextMeshProUGUI StatNumber;

    //Fills in Stat Name and Number, Showing Up To 1 Decimal Place, +/- Signs, and % Sign if Needed
    public void AssignStatInfo(string nameOfStat, float amount, bool isPercentage)
    {
        StatName.text = nameOfStat;
        StatNumber.text = (amount > 0 ? "+" : "") + amount.ToString("0.#") + (isPercentage ? "%" : "");
    }

}
