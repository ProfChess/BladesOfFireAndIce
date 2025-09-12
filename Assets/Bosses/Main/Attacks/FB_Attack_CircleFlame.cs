using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_Attack_CircleFlame : BaseFinalBossAttack
{
    //Stats
    [Header("Attack Specs")]
    [SerializeField] private int CircleSpawnAmount = 3;
    [SerializeField] private float NextCircleSpawnTime = 2f;
    [SerializeField] private float CircleEffectDuration = 5f;
    [SerializeField] private float CircleRotationSpeed = 90f;


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
        for (int i = 0; i < CircleSpawnAmount; ++i)
        {
            SpawnSpell();
            yield return new WaitForSeconds(NextCircleSpawnTime);
            BossAnimator.SetTrigger(SpellTrigger);
            yield return new WaitForSeconds(animWaitTime);
        }
        AttackRoutine = null;
    }

    private void SpawnSpell()
    {
        //Get Object
        GameObject FlameCircle = PoolManager.Instance.getObjectFromPool(EnemyType.CircleFlames);

        //Get Object Script
        ShrinkingCircleDamage CircleScript = FlameCircle.GetComponent<ShrinkingCircleDamage>();

        //Move Object
        FlameCircle.transform.position = transform.position;

        //Set Speed and Duration
        CircleScript.SetStats(CircleRotationSpeed, CircleEffectDuration);

        //Begin Rotation and Shrinking
        CircleScript.BeginEffect();
    }
}
