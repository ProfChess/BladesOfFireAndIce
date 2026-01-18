using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite OpenedChest;
    [SerializeField] private Sprite UnopenedChest;
    [SerializeField] private ChestInventory inventory;
    
    public override void Interact()
    {
        if (sr.sprite == UnopenedChest) { sr.sprite = OpenedChest; }
        ChestLoot loot = inventory.GetRandomLootFromInventory();
        LootBase LootObject = Instantiate(loot.ChestLootObject, transform.position, Quaternion.identity);
        ThrowLootRandomDirection(LootObject.gameObject);
    }
    public void AssignLoottable(ChestInventory Inventory)
    {
        inventory = Inventory;
    }

    //Throwing Loot
    private void ThrowLootRandomDirection(GameObject LootObject)
    {
        Vector2 Dir = GetRandomDirection();
        float Power = Random.Range(2f, 4f);
        Vector2 TargetLocation = (Vector2)gameObject.transform.position + (Dir * Power);

        if (LootObject.TryGetComponent<LootBase>(out LootBase loot))
        {
            loot.beginMove(gameObject.transform.position, TargetLocation);
        }
    }
    private Vector2 GetRandomDirection()
    {
        float RandomAngle = Random.Range(0f, 360f);
        float angleInRad = RandomAngle * Mathf.Deg2Rad;
        
        //Direction = (cos(angle), sin(angle))
        Vector2 Direction = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
        return Direction;
    }
}

