using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =("Effect/Relic"))]
public class Relic : BaseBoon
{
    public RelicEffectType EffectType;
}
public enum RelicEffectType { HealthBuff, DamageBuff, AttackSpeedBuff, MoveSpeedBuff}
public class RelicBaseStats
{

}