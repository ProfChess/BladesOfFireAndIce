using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LevelObjects/ChestData")]
public class ChestInventory : ScriptableObject
{
    [SerializeField] private List<ChestLoot> ChestLootComplete = new();
    public IReadOnlyList<ChestLoot> ChestLootAll => ChestLootComplete;

    public ChestLoot GetRandomLootFromInventory()
    {
        int Choice = Random.Range(0, ChestLootComplete.Count);
        return ChestLootComplete[Choice];
    }
}
[System.Serializable]
public class ChestLoot
{
    public float chanceToAppear = 0f;
    public LootBase ChestLootObject;
    public void PickupLoot() { }
}

