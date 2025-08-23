using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FB_Attack_FlameSpawn : BaseFinalBossAttack
{
    [Header("Attack Details")]
    [SerializeField] private int Columns = 5;
    [SerializeField] private int Rows = 4;
    [SerializeField] private int NumberOfWaves = 3;
    [SerializeField] private int FlamesInEachWave = 10;
    [SerializeField] private float timeBetweenWaves = 0.5f;
    [SerializeField] private float timeBetweenFlames = 0.25f;
    private const float animWaitTime = 1;
    public override void StartAttack(BossAttackOption AttackOption)
    {

        base.StartAttack(AttackOption);
    }

    //Create Attack Coroutine Here
    protected override IEnumerator SpellCastRoutine()
    {
        yield return StartCoroutine(BossTeleport.TeleportCorner());
        BossAnimator.SetTrigger(SpellTrigger);
        yield return new WaitForSeconds(animWaitTime);
        Vector2[] RoomPositions = FB_RoomInfo.Instance.GetRoomCellPositions(Columns, Rows);

        //Spawn the Waves
        for (int i = 0; i < NumberOfWaves; ++i)
        {
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
        GameObject Flame = PoolManager.Instance.getObjectFromPool(EnemyType.Flames);
        if (Flame != null) { Flame.transform.position = Pos; }
    }




}
