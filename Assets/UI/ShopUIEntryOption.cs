using UnityEngine;
using UnityEngine.UI;

public class ShopUIEntryOption : MonoBehaviour
{
    [SerializeField] private Text ShopItemName;
    [SerializeField] private Text ShopItemCostTextField;
    [SerializeField] private Image Icon;
    [SerializeField] private GameObject Sold;
    private bool _isSold = false;
    private ShopOption CurrentOption;

    public void Assign(ShopOption Option)
    {
        ShopItemName.text = Option.Description.ItemName;
        ShopItemCostTextField.text = Option.Description.ItemCost.ToString();
        Icon = Option.Description.Icon;
        CurrentOption = Option;
        if (Option.isSold) { _isSold = true; Sold.SetActive(true); }
        gameObject.SetActive(true);
    }

    public void Select()
    {
        if (_isSold) { return; }
        GameManager.Instance.MakeShopDecision(CurrentOption);
        gameObject.SetActive(false);
    }

}
