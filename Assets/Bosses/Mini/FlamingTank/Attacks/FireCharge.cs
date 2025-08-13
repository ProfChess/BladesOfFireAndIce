using System.Collections;
using System.Collections.Generic;
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

    //Temp Saved Settings
    private float storedSpeed = 0;
    private float storedStoppingDistance = 0;

    protected override void Start()
    {
        base.Start();
        Player = GameManager.Instance.getPlayer();
        BossAgent = BossRef.GetAgent();
    }
    //Animation
    private static readonly int ChargeTrigger = Animator.StringToHash("ChargeTrigger");
    private static readonly int ChargeEndTrigger = Animator.StringToHash("ChargeLoopEnd");
    public override void StartAttack(BossAttackOption AttackOption)
    {
        //Save Settings
        SaveNavSettings();

        //Get Location To Go
        Vector3 TargetToTravel = Player.transform.position;

        if (BossAgent == null) { BossAgent = BossRef.GetAgent(); }
        if (AttackRoutine == null)
        {
            BossAnim.SetTrigger(ChargeTrigger);
            AttackRoutine = StartCoroutine(DashForDuration(TargetToTravel));
        }
        base.StartAttack(AttackOption);
    }

    private IEnumerator DashForDuration(Vector3 Target)
    {
        TempDashSettings();
        BossAgent.SetDestination(GetPointOnMesh(Target));

        while (BossAgent.pathPending || BossAgent.remainingDistance >= 0.1f)
        {
            yield return null; //Wait till boss reaches destination
        }
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
}
