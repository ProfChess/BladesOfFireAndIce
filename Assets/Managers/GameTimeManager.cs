using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameTimeManager
{
    public static bool isPaused { get; private set; } = false;

    public static float GameTime
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
}
