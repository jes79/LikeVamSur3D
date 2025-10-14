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

    public static IEnumerator ParabolaMove(Transform obj, Vector3 start, Vector3 end, 
                                            float height, float duration)
    {
        float time = 0.0f;
        while (time < duration)
        {
            float t = time/ duration;
            Vector3 flatPos = Vector3.Lerp(start, end,t);
            
            float y = Mathf.Sin(Mathf.PI*t)*height;
            obj.position = new Vector3(flatPos.x, flatPos.y + y, flatPos.z);

            time += Time.deltaTime;
            yield return null;
        }

        obj.position = end; //혹시 모르니깐 최종적으로 end 위치로 이동
    }

}
