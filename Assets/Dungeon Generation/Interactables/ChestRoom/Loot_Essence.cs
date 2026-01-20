using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot_Essence : LootBase
{
    [SerializeField] private float EssenceAmount = 0f;
    public override void Interact()
    {
        base.Interact();
        GameManager.Instance.runData.AddStatCurrency(EssenceAmount);
        gameObject.SetActive(false);
    }
}
