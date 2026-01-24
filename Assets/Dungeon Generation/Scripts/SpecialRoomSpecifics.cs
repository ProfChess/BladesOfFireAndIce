using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRoomSpecifics : MonoBehaviour
{
    [Header("Chest Room Objects")]
    [SerializeField] private Chest Chest;
    [SerializeField] private Transform ChestParent;

    [Header("Shop Room Object")]
    [SerializeField] private Shop ShopKeeper;
    [SerializeField] private Transform ShopParent;

    [Header("Puzzle Objects")]
    [SerializeField] List<BasePuzzleChest> PuzzlePrefabs = new();
    [SerializeField] Transform PuzzleParent;


    public void SpawnSpecialRoomChoice(SpecialRoomKind RoomKind, SpecialDungeonRoom room)
    {
        switch (RoomKind)
        {
            case SpecialRoomKind.None: return;
            case SpecialRoomKind.Chest: ChestRoom(room); break;
            case SpecialRoomKind.Challenge: break;
            case SpecialRoomKind.Buff: break;
            case SpecialRoomKind.Puzzle: PuzzleRoom(room); break;
            case SpecialRoomKind.Shop: ShopRoom(room); break;
        }
    }
    private void ChestRoom(SpecialDungeonRoom room)
    {
        Instantiate(Chest, (Vector3Int)room.CenterPoint, Quaternion.identity, ChestParent);
    }
    private void ShopRoom(SpecialDungeonRoom room)
    {
        Instantiate(ShopKeeper, (Vector3Int)room.CenterPoint, Quaternion.identity, ShopParent);
    }
    private void PuzzleRoom(SpecialDungeonRoom room)
    {
        int choice = Random.Range(0, PuzzlePrefabs.Count);
        BasePuzzleChest PuzzlePrefab = PuzzlePrefabs[choice];
        BasePuzzleChest PuzzleInstance = Instantiate(PuzzlePrefab, (Vector3Int)room.CenterPoint, Quaternion.identity, PuzzleParent);
        PuzzleInstance.SetupChest();
    }
}
