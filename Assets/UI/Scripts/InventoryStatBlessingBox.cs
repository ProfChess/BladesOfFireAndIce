using TMPro;
using UnityEngine;

public class InventoryStatBlessingBox : MonoBehaviour
{
    private StatBlessing EquippedBlessing;
    public MainStatType Stat;
    [SerializeField] private TextMeshProUGUI NumText;
    public void AssignItem(StatBlessing Blessing)
    {
        EquippedBlessing = Blessing;
    }
    public void AssignDisplayNumber(int num)
    {
        NumText.text = num.ToString();
    }
    public void OnClick()
    {
        if (GameManager.Instance != null)
        {
            int CurrentStatNum = GameManager.Instance.statManager.GetStatPointsFromStat(Stat);
            if (CurrentStatNum < 10 | EquippedBlessing == null) { return; }

            //Display Stored Blessing Information
            GameManager.Instance.uiManager.InventoryUIObject.DisplayItemDetails(EquippedBlessing);
        }
    }
}
