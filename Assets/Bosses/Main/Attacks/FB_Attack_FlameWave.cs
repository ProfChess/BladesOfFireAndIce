using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_Attack_FlameWave : BaseFinalBossAttack
{
    [Header("Attack Specs")]
    [SerializeField] private float WaveMoveSpeed = 4f;
    [SerializeField] private float WaveMoveDuration = 3f;
    [SerializeField] private float WaveSpawnNumber = 3f;
    [SerializeField] private float WaveInbetweenTime = 1f;

    //Other Stats
    private float AnimWaitTime = 0.75f;

    //Quick Access
    private GameObject Player => GameManager.Instance.getPlayer();

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
            yield return new WaitForSeconds(WaveInbetweenTime);
        }
        AttackRoutine = null;
    }
    private void SpawnSpell()
    {
        //Get Object
        GameObject FlameWave = BossPoolManager.Instance.getObjectFromPool(BossAttackPrefabType.FlameWaves);

        //Place Object
        FlameWave.transform.position = gameObject.transform.position;
        Vector2 Direction = Player.transform.position - transform.position;

        //Set and Start Object
        FlameWave.GetComponent<FlameWaveDamage>().BeginMovement(Direction, WaveMoveSpeed, WaveMoveDuration);
    }
}
