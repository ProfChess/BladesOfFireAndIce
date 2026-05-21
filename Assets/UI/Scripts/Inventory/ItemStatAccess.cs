using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemStatAccess : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI StatName;
    [SerializeField] private TextMeshProUGUI StatNumber;

    //Fills in Stat Name and Number, Showing Up To 1 Decimal Place, +/- Signs, and % Sign if Needed
    public void AssignStatInfo(string nameOfStat, float amount, bool isPercentage, float? oldAmount = null)
    {
        StatName.text = nameOfStat;
        if (oldAmount.HasValue) //In the Case of Virtues, They can display their old amount dependant on the UI
        {
            StatNumber.text = "(" + GetSpecificStringFromAmount(oldAmount.Value, isPercentage) + ")" + "<voffset=0.1em>→</voffset>"
                + GetSpecificStringFromAmount(amount, isPercentage);
        }
        else
        {
            StatNumber.text = GetSpecificStringFromAmount(amount, isPercentage);
        }
    }
    private string GetSpecificStringFromAmount(float amount, bool isPercentage)
    {
        return (amount > 0 ? "+" : "") + amount.ToString("0.#") + (isPercentage ? "%" : "");
    }
    public void ClearText()
    {
        StatName.text = "";
        StatNumber.text = "";
    }
    public bool isEmpty() { return StatName.text == ""; }
    public void SetFontSize(float size)
    {
        StatName.fontSize = size;
        StatNumber.fontSize = size;
    }
}
