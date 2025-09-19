using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_Attack_FlameWave : BaseFinalBossAttack
{
    [Header("Attack Specs")]
    [SerializeField] private float WaveMoveSpeed = 4f;
    [SerializeField] private float WaveSpawnNumber = 3f;
    [SerializeField] private float WaveInbetweenTime = 1f;

    //Other Stats
    private float AnimWaitTime = 0.4f;

    public override void StartAttack(BossAttackOption AttackOption)
    {
        base.StartAttack(AttackOption);
    }
    protected override IEnumerator SpellCastRoutine()
    {
        yield return StartCoroutine(BossTeleport.TeleportCardinal());
        for (int i = 0; i < WaveSpawnNumber; i++)
        {
            BossAnimator.SetTrigger(SpellTrigger);
            yield return new WaitForSeconds(AnimWaitTime);
            SpawnSpell();
            yield return new WaitForSeconds(1.1f - AnimWaitTime);
        }
        AttackRoutine = null;
    }
    private void SpawnSpell()
    {

    }
}
