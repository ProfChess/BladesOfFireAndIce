using UnityEngine;
using UnityEngine.UI;

public class ShopUIEntryOption : MonoBehaviour
{
    [SerializeField] private Text ShopItemName;
    [SerializeField] private Text ShopItemCostTextField;
    [SerializeField] private Image Icon;

    private ShopOption CurrentOption;

    public void Assign(ShopOption Option)
    {
        ShopItemName.text = Option.Description.ItemName;
        ShopItemCostTextField.text = Option.Description.ItemCost.ToString();
        Icon = Option.Description.Icon;
        gameObject.SetActive(true);
    }

    public void Select()
    {
        GameManager.Instance.MakeShopDecision(CurrentOption);
    }

}
