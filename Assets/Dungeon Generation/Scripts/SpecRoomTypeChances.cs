using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("DungeonGeneration/SpecialRoomDataSet"))]
public class SpecRoomTypeChances : ScriptableObject
{
    public List<SpecialRoomTypeChance> RoomChances = new List<SpecialRoomTypeChance>();
}
//Struct for holding room appearance chances
[System.Serializable]
public struct SpecialRoomTypeChance
{
    public SpecialRoomKind Type;
    [Range(0f, 1f)] public float Chance;
    public int AppearanceLimit;
}
