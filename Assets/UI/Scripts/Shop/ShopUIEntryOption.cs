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
    private bool hasSelected = false;
    private bool isSold = false;

    //Assigns Attributes Based on Given Info
    public void Assign(ShopOption Option)
    {
        ShopItemName.text = Option.item.BonusName;
        ShopItemCostTextField.text = Option.itemCost.ToString();
        TypeText.text = Option.item.type.ToString();
        //Icon = Option.item.Icon;
        CurrentOption = Option;
        isSold = false;

        CheckEntryStatus();

        //Activate Object
        gameObject.SetActive(true);
    }
    //Checks and Updates Overlays Based on If Item is Sold/Too Expensive/Normal
    public void CheckEntryStatus()
    {
        //Item Sold
        SoldOverlay.SetActive(isSold);
        if (isSold) { ShopItemCostTextField.color = Color.white; }

        //Item Price
        else
        {
            _isAffordable = CurrentOption.CanAfford(GameManager.Instance.runData.GoldCurrencyCollected);
            ShopItemCostTextField.color = _isAffordable ? Color.white : CantAffordColor;
        }
    }

    //Alerts Game Manager When Item is Chosen
    public void OnClick_SelectOption()
    {
        if (hasSelected)
        {
            hasSelected = false;
            PurchaseItem();
            return;
        }
        SelectItem();
    }
    private void SelectItem()
    {
        if (isSold) { return; }
        GameManager.Instance.uiManager.ShopUIObject.AssignItemDetailsFromShopOption(CurrentOption);
        hasSelected = true;
    }
    private void PurchaseItem()
    {
        if (isSold || !_isAffordable) { return; }
        GameManager.Instance.uiManager.MakeShopDecision(CurrentOption);
        isSold = true;
        CheckEntryStatus();
    }
    public void DeselectItem() { hasSelected = false; }
    private void OnEnable()
    {
        hasSelected = false;
    }
}
