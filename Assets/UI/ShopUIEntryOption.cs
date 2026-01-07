using UnityEngine;
using UnityEngine.UI;

public class ShopUIEntryOption : MonoBehaviour
{
    [SerializeField] private Text ShopItemName;
    [SerializeField] private Text ShopItemCostTextField;
    [SerializeField] private Image Icon;
    [SerializeField] private GameObject SoldOverlay;
    [SerializeField] private GameObject CantAffordOverlay;
    private bool _isAffordable = true;
    private ShopOption CurrentOption;

    //Assigns Attributes Based on Given Info
    public void Assign(ShopOption Option)
    {
        //Fill in Item Attributes
        ShopItemName.text = Option.Description.ItemName;
        ShopItemCostTextField.text = Option.Description.ItemCost.ToString();
        Icon = Option.Description.Icon;
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
        if (CurrentOption.isSold) { CantAffordOverlay.SetActive(false); }
        
        //Item Price
        else
        {
            _isAffordable = GameManager.Instance.runData.CanPayItemCost(CurrentOption.Description.ItemCost);
            CantAffordOverlay.SetActive(!_isAffordable);
        }
    }

    //Alerts Game Manager When Item is Chosen
    public void Select()
    {
        if (CurrentOption.isSold || !_isAffordable) { return; }
        GameManager.Instance.MakeShopDecision(CurrentOption);
        CurrentOption.isSold = true; SoldOverlay.SetActive(true);
    }

}
