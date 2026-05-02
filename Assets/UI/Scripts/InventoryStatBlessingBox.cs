using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryStatBlessingBox : MonoBehaviour
{
    private StatBlessing EquippedBlessing;
    public MainStatType Stat;
    [SerializeField] private TextMeshProUGUI NumText;
    [SerializeField] private Button ButtonComponent;

    StatManager SM => GameManager.Instance.statManager;
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
            int CurrentStatNum = SM.GetStatPointsFromStat(Stat);
            if (CurrentStatNum < SM.MaxStatPoints | EquippedBlessing == null) { return; }

            //Display Stored Blessing Information
            GameManager.Instance.uiManager.InventoryUIObject.DisplayItemDetails(EquippedBlessing);
        }
    }
    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            int CurrentStatNum = SM.GetStatPointsFromStat(Stat);
            ButtonComponent.interactable = CurrentStatNum >= SM.MaxStatPoints;
        }
    }
}
