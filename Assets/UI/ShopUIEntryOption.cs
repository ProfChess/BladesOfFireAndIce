using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIEntryOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ShopItemName;
    [SerializeField] private TextMeshProUGUI ShopItemCostTextField;
    [SerializeField] private TextMeshProUGUI TypeText;
    [SerializeField] private Image Icon;
    [SerializeField] private GameObject SoldOverlay;
    [SerializeField] private Color CantAffordColor;
    private bool _isAffordable = true;
    private ShopOption CurrentOption;

    //Assigns Attributes Based on Given Info
    public void Assign(ShopOption Option)
    {
        ShopItemName.text = Option.item.BonusName;
        ShopItemCostTextField.text = Option.itemCost.ToString();
        //Icon = Option.item.Icon;
        CurrentOption = Option;

        CheckEntryStatus();

        //Activate Object
        gameObject.SetActive(true);
    }
    //Checks and Updates Overlays Based on If Item is Sold/Too Expensive/Normal
    public void CheckEntryStatus()
    {
        //Item Sold
        SoldOverlay.SetActive(CurrentOption.isSold);
        if (CurrentOption.isSold) { ShopItemCostTextField.color = Color.white; }
        
        //Item Price
        else
        {
            _isAffordable = CurrentOption.CanAfford(GameManager.Instance.runData.GoldCurrencyCollected);
            ShopItemCostTextField.color = _isAffordable ? Color.white : CantAffordColor;
        }
    }

    //Alerts Game Manager When Item is Chosen
    public void Select()
    {
        if (CurrentOption.isSold || !_isAffordable) { return; }
        GameManager.Instance.MakeShopDecision(CurrentOption);
        CurrentOption.isSold = true;
        CheckEntryStatus();
    }

}
