using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FB_Attack_FlameSpawn : BaseFinalBossAttack
{
    [Header("Attack Specs")]
    [SerializeField] private int Columns = 5;
    [SerializeField] private int Rows = 4;
    [SerializeField] private int NumberOfWaves = 3;
    [SerializeField] private int FlamesInEachWave = 10;
    [SerializeField] private float timeBetweenWaves = 0.5f;
    [SerializeField] private float timeBetweenFlames = 0.25f;
    [SerializeField] private float FlameOffsetMin = 0f;
    [SerializeField] private float FlameOffsetMax = 3f;
    private const float animWaitTime = 0.6f;
    
    public override void StartAttack(BossAttackOption AttackOption)
    {
        base.StartAttack(AttackOption);
    }

    //Create Attack Coroutine Here
    protected override IEnumerator SpellCastRoutine()
    {
        yield return StartCoroutine(BossTeleport.TeleportCorner());
        Vector2[] RoomPositions = FB_RoomInfo.Instance.GetRoomCellPositions(Columns, Rows);

        //Spawn the Waves
        for (int i = 0; i < NumberOfWaves; ++i)
        {
            BossAnimator.SetTrigger(SpellTrigger);
            yield return new WaitForSeconds(animWaitTime);
            yield return StartCoroutine(FlameWaveSpawn(FlamesInEachWave, RoomPositions));
            yield return new WaitForSeconds(timeBetweenWaves);  
        }
        AttackRoutine = null; //End of Routine
    }
    private IEnumerator FlameWaveSpawn(int FlamesPerWave, Vector2[] Positions)
    {
        List<Vector2> PositionsStillValid = Positions.ToList<Vector2>();
        if (FlamesPerWave > Positions.Length) { FlamesPerWave = Positions.Length; }

        for (int i = 0; i < FlamesPerWave; ++i)
        {
            int ChosenNum = Random.Range(0, PositionsStillValid.Count);
            Vector2 ChosenPos = PositionsStillValid[ChosenNum];
            PositionsStillValid.RemoveAt(ChosenNum);
            SpawnFlame(ChosenPos);
            yield return new WaitForSeconds(timeBetweenFlames);  
        }
    }
    private void SpawnFlame(Vector2 Pos)
    {
        //Select Random Location
        int dirOptions = Random.Range(0, 4);
        Vector2 DirectionSelection = Vector2.zero;
        switch (dirOptions)
        {
            case 0: DirectionSelection = Vector2.up; break;
            case 1: DirectionSelection = Vector2.down; break;
            case 2: DirectionSelection = Vector2.left; break;
            case 3: DirectionSelection = Vector2.right; break;
        }

        float RandomOffset = Random.Range(FlameOffsetMin, FlameOffsetMax);
        Vector2 spawnPos = Pos + DirectionSelection * RandomOffset;

        GameObject Boom = PoolManager.Instance.getObjectFromPool(EnemyType.SmallExplosions);
        if (Boom != null) 
        {
            Boom.transform.position = spawnPos;
            Boom.GetComponent<SmallExplosion>().StartExplosion(); 
        }
    }




}
