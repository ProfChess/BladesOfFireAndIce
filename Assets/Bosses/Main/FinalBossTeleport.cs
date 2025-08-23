using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FinalBossTeleport : MonoBehaviour
{
    private NavMeshAgent BossAgent;

    [Header("Animation")]
    [SerializeField] private Animator BossAnim;
    private static readonly int StartTeleportTrig = Animator.StringToHash("TeleportStartTrigger");
    private static readonly int EndTeleportTrig = Animator.StringToHash("TeleportEndTrigger");
    private const float teleportAnimWaitTime = 1.1f;

    [Header("References")]
    [SerializeField] private Collider2D hitbox;

    private void Start()
    {
        BossAgent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator TeleportRoutine(Vector2 Destination)
    {
        BossAnim.SetTrigger(StartTeleportTrig);
        yield return new WaitForSeconds(teleportAnimWaitTime);
        hitbox.enabled = false;
        BossAgent.SetDestination(Destination);

        while (BossAgent.remainingDistance - BossAgent.stoppingDistance > 0)
        {
            yield return null;
        }

        BossAnim.SetTrigger(EndTeleportTrig);
        yield return new WaitForSeconds(0.5f);
        hitbox.enabled = true;

    }


    public IEnumerator TeleportCorner()
    {
        Vector2 ChosenPoint = FB_RoomInfo.Instance.GetRandomCornerPoint(gameObject.transform.position);
        yield return StartCoroutine(TeleportRoutine(ChosenPoint));
    }
    public IEnumerator TeleportMiddle()
    {
        Vector2 ChosenPoint = FB_RoomInfo.Instance.GetMiddlePoint();
        yield return StartCoroutine(TeleportRoutine(ChosenPoint));
    }
    public IEnumerator TeleportCardinal()
    {
        Vector2 ChosenPoint = FB_RoomInfo.Instance.GetRandomCardinalPoint(gameObject.transform.position);
        yield return StartCoroutine(TeleportRoutine(ChosenPoint));
    }

}
