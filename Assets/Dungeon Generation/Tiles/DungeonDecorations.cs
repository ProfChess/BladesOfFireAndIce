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

    [Tooltip("How Far can the Object Offset From its Given Position (Between 0 - 0.5)")]
    [SerializeField] private Vector2 PotentialOffset = new Vector2(0,0);

    [Tooltip("Controls if Decoration can Spawn Along any Wall or Only at Upper Corners")]
    public bool cornerOnly = false;

    public SpawnParentType ParentType;
    public Vector2 GetPotentialOffset => new Vector2(Mathf.Clamp(PotentialOffset.x, 0f, 0.5f), Mathf.Clamp(PotentialOffset.y, 0f, 0.5f));
}
public enum SpawnParentType { WallChain1, WallChain2, WallChain3, Candle1, Candle2, Candle3, Box1, Box2, Box3, Box4 };
