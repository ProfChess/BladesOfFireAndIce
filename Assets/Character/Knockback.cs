using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    private float KnockbackDuration = 0.065f;
    private Coroutine KnockbackCoroutine;
    public void SetupKnockback(NavMeshAgent NavAgent)
    {
        agent = NavAgent;
    }

    //House for Knockback Logic on Enemies
    public void KnockbackObject(Vector2 Direction, float Power)
    {
        if (agent == null)
        {
            agent = gameObject.GetComponentInParent<NavMeshAgent>();
        }

        if (KnockbackCoroutine == null)
        {
            KnockbackCoroutine = StartCoroutine(KnockbackRoutine(Direction * Power));
        }
    }
    private IEnumerator KnockbackRoutine(Vector2 KnockBackDistance)
    {
        float timePassed = 0f;

        //Let Impact Frame Play Before Applying Knockback
        yield return null;

        while (timePassed < KnockbackDuration)
        {
            timePassed += Time.deltaTime;

            Vector2 movement = KnockBackDistance * (Time.deltaTime / KnockbackDuration);
            agent.Move(movement);

            yield return null;
        }


        KnockbackCoroutine = null;
    }
}
