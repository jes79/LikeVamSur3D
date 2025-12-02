using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Skill01_Lightning : SkillBase
{
    private int FireCount; //¹ø°³ °¹¼ö

    protected override void OnInitalize()
    {
        FireCount = level;
    }

    protected override void OnLevelUp()
    {
        FireCount = level;
    }

    protected override void Fire()
    {
        if (targets.Count < 0) return;
        Transform targetPoint = targets[Random.Range(0, targets.Count)];
        for (int i = 0; i < FireCount; i++)
        {
            var lightning = MANAGER.POOL.Pooling_OBJ("Lightning").Get((value) =>
            {
                value.transform.position = targetPoint.position;
                targetPoint.GetComponent<MONSTER>().GetDamage(Damage());
                value.GetComponent<ParticleSystem>().Play();

            });
        }


    }

}
