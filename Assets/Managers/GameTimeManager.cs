using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTimeManager
{
    public static bool isPaused { get; private set; } = false;

    //Use for Things to be UnAffected by HitStop, but Still React When Game is Paused
    //Use for Counting Down in Update Calls 
    public static float GameDeltaTime
    {
        get
        {
            if (isPaused) return 0f;
            return Time.unscaledDeltaTime;
        }
    }
    public static void SetPaused(bool paused)
    {
        isPaused = paused;
    }

    //Use for Waiting in Coroutines
    public static IEnumerator WaitFor(float Duration)
    {
        float timer = 0f;
        while (timer < Duration)
        {
            timer += GameDeltaTime;
            yield return null;
        }
    }
}
