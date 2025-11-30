using UnityEngine;

[CreateAssetMenu(menuName = "DungeonGeneration/Tile Set")]
public class TileSet : ScriptableObject
{
    [Header("Floor")]
    public RuleTile DefaultFloor;

    [Header("Wall")]
    [Header("Wall Bases")]
    public RuleTile WallEmpty;
    public RuleTile WallBottomMid;
    public RuleTile WallBottomRight;
    public RuleTile WallBottomLeft;

    [Header("Wall Corners")]
    [Header("Outer")]
    public RuleTile WallOuterCornerNE;
    public RuleTile WallOuterCornerNW;
    public RuleTile WallOuterCornerSW;
    public RuleTile WallOuterCornerSE;
    [Header("Inner")]
    public RuleTile WallInnerCornerNE;
    public RuleTile WallInnerCornerNW;
    public RuleTile WallInnerCornerSW;
    public RuleTile WallInnerCornerSE;

    [Header("Wall Tops")]
    [Header("Wide")]
    public RuleTile WallTopN;
    public RuleTile WallTopE;
    public RuleTile WallTopS;
    public RuleTile WallTopW;
    [Header("Narrow")]
    public RuleTile WallTopNarrowN;
    public RuleTile WallTopNarrowS;
    public RuleTile WallTopNarrowNS;
    public RuleTile WallNarrowE;
    public RuleTile WallNarrowW;
    public RuleTile WallNarrowEW;
    public RuleTile WallNarrowBase;

}
