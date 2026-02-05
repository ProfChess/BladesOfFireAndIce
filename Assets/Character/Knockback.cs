using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Knockback : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    private Coroutine KnockbackCoroutine;
    public void SetupKnockback(NavMeshAgent NavAgent)
    {
        agent = NavAgent;
    }

    //House for Knockback Logic on Enemies
    public void KnockbackObject(Vector2 Direction, float Power, float Duration, AnimationCurve falloff)
    {
        if (agent == null)
        {
            agent = gameObject.GetComponentInParent<NavMeshAgent>();
        }

        if (KnockbackCoroutine == null)
        {
            KnockbackCoroutine = StartCoroutine(KnockbackRoutine((Direction * Power), Duration, falloff));
        }
    }
    private IEnumerator KnockbackRoutine(Vector2 KnockBackDistance, float Duration, AnimationCurve falloffCurve)
    {
        float timePassed = 0f;
        float lastCurveValue = falloffCurve.Evaluate(0f);
        agent.isStopped = true; 
        //Let Impact Frame Play Before Applying Knockback
        yield return null;

        while (timePassed < Duration)
        {
            timePassed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(timePassed / Duration);
            float curveValue = falloffCurve.Evaluate(t);

            Vector2 movement = KnockBackDistance * (curveValue - lastCurveValue);
            agent.Move(movement);

            lastCurveValue = curveValue;
            yield return null;
        }

        agent.isStopped = false;
        KnockbackCoroutine = null;
    }
}
