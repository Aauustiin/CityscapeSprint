using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public static event System.Action GameOver;

    public static void TriggerGameOver()
    {
        GameOver?.Invoke();
    }

    public static event System.Action Restart;

    public static void TriggerRestart()
    {
        Restart?.Invoke();
    }
}
