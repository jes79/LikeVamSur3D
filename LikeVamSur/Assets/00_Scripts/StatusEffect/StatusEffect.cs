using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    MONSTER monster;
    public GameObject Burn;
    public GameObject FreezeStone;
    [HideInInspector] 
    public Renderer renderer;
    private float freezeStack;

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

    public void ApplyFreeze(float stackAmount)
    {
        var freeze = activeEffects.FirstOrDefault(x => x is Freeze_Status) as Freeze_Status;

        if (freeze != null)
        {
            freeze.AddStack(stackAmount);
            freeze.Apply(monster, this);
        }
        else
        {
            var newFreeze = new Freeze_Status();
            newFreeze.AddStack(stackAmount);
            newFreeze.Apply(monster, this);
            activeEffects.Add(newFreeze);
        }
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
