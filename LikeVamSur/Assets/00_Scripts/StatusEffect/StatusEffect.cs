using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    MONSTER monster;
    public GameObject Burn;

    List<IStatusEffect> activeEffects = new List<IStatusEffect>();
    private void Start()
    {
        monster = GetComponent<MONSTER>();
    }

    public void ApplyBurn()
    {
        //화상은 중첩이 되지 않게 처리할 것이다.
        activeEffects.RemoveAll(e => e is Burn_Status);
        Burn_Status burn = new Burn_Status();
        burn.Apply(monster, this);
        activeEffects.Add(burn);
        
    }

    private void Update()
    {
        for (int i = 0; i < activeEffects.Count; i++)
        {
            activeEffects[i].Tick(monster);
            if (activeEffects[i].IsFinished)
            {
                activeEffects[i].End(monster, this);
                activeEffects.RemoveAt(i);   
            }
        }
    }
}
