using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DungeonGeneration/DecorSet")]
public class DungeonDecorations : ScriptableObject
{
    public float BaseChanceToDecorateWall = 0.1f;
    public List<DungeonDecor> WallDecorations;

    public float BaseChanceToDecorateFloor = 0.1f;
    public List<DungeonDecor> FloorDecorations;
}

[System.Serializable]
public class DungeonDecor
{
    [Tooltip("Object Being Placed")]
    public GameObject Object;

    [Tooltip("Chance for Object to Be Placed in Each Instance Where There is Room")]
    public float SpawnChance = 0f;

    [Tooltip("Amount of Room Needed to Place the Decoration in (x,y)")]
    public Vector2Int RequiredSpace = new Vector2Int(1, 1);

    [Tooltip("Decorations That are not Breakable Will not be Placed Near Entrances/Exits")]
    public bool isBreakable = false;
}