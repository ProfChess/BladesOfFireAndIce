using UnityEngine;

public class Loot_Gold : LootBase
{
    [SerializeField] private float GoldAmount = 0f;
    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.runData.AddShopCurrency(GoldAmount);
        gameObject.SetActive(false);
    }
}
