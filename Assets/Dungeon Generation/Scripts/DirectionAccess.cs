using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DirectionAccess
{
    public static readonly Vector2Int[] CardinalDir = new Vector2Int[]
    {
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1),
        new Vector2Int(0, -1),
    };
    public static readonly Vector2Int[] AllDirections = new Vector2Int[]
    {
        new Vector2Int(1, 0),   //East
        new Vector2Int(1, -1),  //South East
        new Vector2Int(0, -1),  //South
        new Vector2Int(-1, -1), //South West
        new Vector2Int(-1, 0),  //West
        new Vector2Int(-1, 1),  //North West
        new Vector2Int(0, 1),   //North
        new Vector2Int(1, 1),   //North East
    };
}
