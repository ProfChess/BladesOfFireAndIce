using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorationParentSet : MonoBehaviour
{
    [Header("WallChains")]
    [SerializeField] private Transform WallChain1;
    [SerializeField] private Transform WallChain2;
    [SerializeField] private Transform WallChain3;

    [Header("Candles")]
    [SerializeField] private Transform Candle1;
    [SerializeField] private Transform Candle2;
    [SerializeField] private Transform Candle3;

    [Header("Boxes")]
    [SerializeField] private Transform Box1;
    [SerializeField] private Transform Box2;
    [SerializeField] private Transform Box3;
    [SerializeField] private Transform Box4;

    public Transform GetDecorParent(SpawnParentType Parent)
    {
        switch (Parent)
        {
            default: return gameObject.transform;

            case SpawnParentType.Box1:
                return Box1;
            case SpawnParentType.Box2:
                return Box2;
            case SpawnParentType.Box3:
                return Box3;
            case SpawnParentType.Box4:
                return Box4;

            case SpawnParentType.WallChain1:
                return WallChain1;
            case SpawnParentType.WallChain2:
                return WallChain2;
            case SpawnParentType.WallChain3:
                return WallChain3;

            case SpawnParentType.Candle1:
                return Candle1;
            case SpawnParentType.Candle2:
                return Candle2;
            case SpawnParentType.Candle3:
                return Candle3;
        }
    }
}
