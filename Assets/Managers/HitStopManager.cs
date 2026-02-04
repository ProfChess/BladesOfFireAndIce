using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    private Coroutine HitStopCoroutine;
    private float hitStopEndTime;
    private float targetTimeScale = 1f;

    //Hit Stop Call
    public void BeginHitStop()
    {
        float hitStopScale = 1f;
        float hitStopDuration = 0f;
        if (PlayerController.PlayerAttackForm == ElementType.Fire)
        {
            hitStopScale = PlayerController.FireStanceHitStopScale;
            hitStopDuration = PlayerController.FireStanceHitStopDuration;
        }
        if (PlayerController.PlayerAttackForm == ElementType.Ice)
        {
            hitStopScale = PlayerController.IceStanceHitStopScale;
            hitStopDuration = PlayerController.IceStanceHitStopTime;
        }
        HitStop(hitStopScale, hitStopDuration);
    }
    //Continue or Stop Hitstop
    private void HitStop(float scale, float Duration)
    {
        float newEndTime = Time.realtimeSinceStartup + Duration;

        if (scale < targetTimeScale) { targetTimeScale = scale; }
        if (newEndTime > hitStopEndTime) { hitStopEndTime = newEndTime; }

        if (HitStopCoroutine == null)
        {
            HitStopCoroutine = StartCoroutine(HitStopRoutine());
        }
    }
    //Routine for Stopping
    private IEnumerator HitStopRoutine()
    {
        //Delay for 2 Frames
        yield return null;
        yield return null;

        Time.timeScale = targetTimeScale;

        while (Time.realtimeSinceStartup < hitStopEndTime) { yield return null; }

        Time.timeScale = 1f;
        targetTimeScale = 1f;
        HitStopCoroutine = null;
    }
}
