using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    [Header("Chest Specifics")]
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected Sprite OpenedChest;
    [SerializeField] protected Sprite UnopenedChest;
    [SerializeField] protected ChestInventory inventory;
    [SerializeField] protected ChestInventory guaranteedInventory;
    protected bool isOpened = false;

    [Header("Loot Spawning")]
    [SerializeField] protected float minSpawnRange = 1.35f;
    [SerializeField] protected float maxSpawnRange = 2.5f;
    [SerializeField] protected float timeBetweenLootSpawn = 0.25f;
    protected List<ChestLoot> LootToSpawn = new();
    public override void Interact()
    {
        if(isOpened) return;

        isOpened = true;
        if (sr.sprite == UnopenedChest) { sr.sprite = OpenedChest; }

        //List of Loot to Create
        LootToSpawn.Clear();
        //Grant Set Amount of Items from guaranteedInventory
        foreach(ChestLoot ChestEntry in guaranteedInventory.ChestLootAll)
        {
            //Spawn Each Item Once
            LootToSpawn.Add(ChestEntry);
        }

        //Grant Item from Main Inventory
        LootToSpawn.Add(inventory.GetRandomLootFromInventory());

        //Start Coroutine to Spawn Loot
        StartCoroutine(LootSpawnRoutine());
    }
    public void AssignLoottable(ChestInventory Inventory)
    {
        inventory = Inventory;
        isOpened = false;
    }

    protected void SpawnAndThrowLoot(ChestLoot ChestEntry)
    {
        LootBase Loot = Instantiate(ChestEntry.ChestLootObject, transform.position, Quaternion.identity);
        ThrowLootRandomDirection(Loot.gameObject);
    }

    //Throwing Loot
    protected void ThrowLootRandomDirection(GameObject LootObject)
    {
        Vector2 Dir = GetRandomDirection();
        float Power = Random.Range(minSpawnRange, maxSpawnRange);
        Vector2 TargetLocation = (Vector2)gameObject.transform.position + (Dir * Power);

        if (LootObject.TryGetComponent<LootBase>(out LootBase loot))
        {
            loot.beginMove(gameObject.transform.position, TargetLocation);
        }
    }
    protected Vector2 GetRandomDirection()
    {
        float RandomAngle = Random.Range(0f, 360f);
        float angleInRad = RandomAngle * Mathf.Deg2Rad;
        
        //Direction = (cos(angle), sin(angle))
        Vector2 Direction = new Vector2(Mathf.Cos(angleInRad), Mathf.Sin(angleInRad));
        return Direction;
    }

    //Coroutine for Loot Spawning
    protected IEnumerator LootSpawnRoutine()
    {
        foreach (ChestLoot ChestEntry in LootToSpawn)
        {
            SpawnAndThrowLoot(ChestEntry);
            yield return new WaitForSeconds(timeBetweenLootSpawn);
        }
    }
}

