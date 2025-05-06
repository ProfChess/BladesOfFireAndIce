using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasterVisuals : MonoBehaviour
{
    [SerializeField] private CasterEnemy caster;

    public void TryTeleporting() { caster.AttemptTeleport(); }
    public void MagicAttackSpawn() { caster.SpawnMagicAttack(); }
    public void MagicAttackCD() { caster.BeginMagicAttackCD(); }
    public void MagicAttackTeleportCD() { caster.BeginMATeleportCD(); }
}
