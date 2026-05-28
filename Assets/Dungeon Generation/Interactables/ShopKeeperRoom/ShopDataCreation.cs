using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("LevelObjects/ShopData"))]
public class ShopDataCreation : ScriptableObject
{
    private List<ShopOption> shopOptions = new List<ShopOption>();
    public IReadOnlyList<ShopOption> ShopOptions => shopOptions;
    [SerializeField] private List<ShopOption> boonShopOptions = new();
    [SerializeField] private List<ShopOption> relicShopOptions = new();

    private void OnEnable()
    {
        shopOptions.Clear();

        shopOptions.AddRange(boonShopOptions);
        shopOptions.AddRange(relicShopOptions);
    }
}
