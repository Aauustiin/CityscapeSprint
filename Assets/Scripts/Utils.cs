using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static IEnumerator ExecuteAfterSeconds(System.Action executable, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        executable();
    }

    public static IEnumerator ExecuteWhenTrue(System.Action executable, bool condition)
    {
        yield return new WaitUntil(() => condition);
        executable();
    }
}
