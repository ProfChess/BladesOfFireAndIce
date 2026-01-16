using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="LevelObjects/ChestData")]
public class ChestInventory : ScriptableObject
{
    [SerializeField] private List<ChestLoot_Boon> ChestBoons = new();
    [SerializeField] private List<ChestLoot_TempBuff> ChestBuffs = new();
    private List<ChestLoot> ChestLootComplete = new();
    public IReadOnlyList<ChestLoot> ChestLootAll => ChestLootComplete;

    private void OnEnable()
    {
        ChestLootComplete.Clear();
        ChestLootComplete.AddRange(ChestBoons);
        ChestLootComplete.AddRange(ChestBuffs);
    }
}
[System.Serializable]
public class ChestLoot
{
    public float chanceToAppear = 0f;
}
[System.Serializable]
public class ChestLoot_Boon : ChestLoot
{
    public BaseBoon BoonRef;
}
[System.Serializable]
public class ChestLoot_TempBuff : ChestLoot
{
    public BaseTempBuff TempBuffRef;
}
public class ChestLoot_Relic : ChestLoot
{

}
