using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRoomSpecifics : MonoBehaviour
{
    [SerializeField] private Transform TempParentObject;

    [Header("Chest Room Objects")]
    [SerializeField] private GameObject Chest;

    public void SpawnSpecialRoomChoice(SpecialRoomKind RoomKind, SpecialDungeonRoom room)
    {
        switch (RoomKind)
        {
            case SpecialRoomKind.None: return;
            case SpecialRoomKind.Chest: ChestRoom(room); break;
            case SpecialRoomKind.Challenge: break;
            case SpecialRoomKind.Buff: break;
            case SpecialRoomKind.Puzzle: break;
        }
    }
    private void ChestRoom(SpecialDungeonRoom room)
    {
        Instantiate(Chest, (Vector3Int)room.CenterPoint, Quaternion.identity, TempParentObject);
    }
}
