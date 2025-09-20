using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class FireCharge : BaseBossAttack
{
    [SerializeField] private float ChargeSpeed;
    [SerializeField] private float ChargeTravelDuration;

    [Header("References")]
    [SerializeField] private Animator BossAnim;
    private GameObject Player;
    private Coroutine AttackRoutine;
    private NavMeshAgent BossAgent;

    [Header("Attack VFX")]
    [SerializeField] private Animator ChargeAnim;
    private static readonly int StartChargeEffectTrig = Animator.StringToHash("StartChargeTrigger");
    private static readonly int EndChargeEffectTrig = Animator.StringToHash("EndChargeTrigger");

    //Temp Saved Settings
    private float storedSpeed = 0;
    private float storedStoppingDistance = 0;

    //Flame Locations
    private static readonly float FlameFrequency = 0.15f;
    private float FlameSpawnTimer = 0f;

    protected override void Start()
    {
        base.Start();
        Player = GameManager.Instance.getPlayer();
    }
    //Animation
    private static readonly int ChargeTrigger = Animator.StringToHash("ChargeTrigger");
    private static readonly int ChargeEndTrigger = Animator.StringToHash("ChargeLoopEnd");
    public override void StartAttack(BossAttackOption AttackOption)
    {
        if (BossAgent == null) { BossAgent = BossRef.GetAgent(); }

        //Save Settings
        SaveNavSettings();

        //Get Location To Go
        Vector3 TargetToTravel = Player.transform.position;

        if (AttackRoutine == null)
        {
            BossAnim.SetTrigger(ChargeTrigger);
            AttackRoutine = StartCoroutine(DashForDuration(TargetToTravel));
        }
        base.StartAttack(AttackOption);
    }

    private IEnumerator DashForDuration(Vector3 Target)
    {
        FlameSpawnTimer = 0;
        TempDashSettings();
        ChargeAnim.SetTrigger(StartChargeEffectTrig);
        BossAgent.SetDestination(GetPointOnMesh(Target));
        while (BossAgent.pathPending || BossAgent.remainingDistance >= 0.1f)
        {
            FlameSpawnTimer += Time.deltaTime;
            if (FlameSpawnTimer >= FlameFrequency) 
            { 
                SpawnFlames();
                FlameSpawnTimer = 0;
            }
            yield return null; //Wait till boss reaches destination
        }
        ChargeAnim.SetTrigger(EndChargeEffectTrig);
        BossAnim.SetTrigger(ChargeEndTrigger);
        yield return new WaitForSeconds(0.2f);
        RestoreNavSettings();

        AttackRoutine = null;
    }

    //Get Point on Mesh
    protected Vector2 GetPointOnMesh(Vector3 Point)
    {
        if (NavMesh.SamplePosition(Point, out NavMeshHit hit, Mathf.Infinity, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return Vector2.zero;
    }
    private void TempDashSettings()
    {
        BossAgent.speed = ChargeSpeed;
        BossAgent.stoppingDistance = 0;
    }
    private void RestoreNavSettings() 
    {
        BossAgent.speed = storedSpeed;
        BossAgent.stoppingDistance = storedStoppingDistance;
    }
    private void SaveNavSettings()
    {
        storedSpeed = BossAgent.speed;
        storedStoppingDistance = BossAgent.stoppingDistance;
    }


    //Spawning Flames
    private void SpawnFlames()
    {
        GameObject FlameObj = BossPoolManager.Instance.getObjectFromPool(BossAttackPrefabType.LingeringFlames);
        FlameObj.transform.position = gameObject.transform.position;
    }
}
