using System;
using System.Collections;
using UnityEngine;

public class Util_Coroutine
{
    public static IEnumerator Delay(float delay, Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}
