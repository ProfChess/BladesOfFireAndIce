using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_Attack_CircleFlame : BaseFinalBossAttack
{
    //Stats
    [Header("Attack Specs")]
    [SerializeField] private float NextCircleSpawnTime = 2f;
    [SerializeField] private float CircleShrinkSpeed = 5f;
    [SerializeField] private float CircleRotationSpeed = 5f;


    //Animation
    private const float animWaitTime = 0.5f;

    public override void StartAttack(BossAttackOption AttackOption)
    {
        base.StartAttack(AttackOption);
    }
    protected override IEnumerator SpellCastRoutine()
    {
        yield return StartCoroutine(BossTeleport.TeleportMiddle());
        BossAnimator.SetTrigger(SpellTrigger);
        yield return new WaitForSeconds(animWaitTime);

        //Spawn Attack Logic
    }
}
