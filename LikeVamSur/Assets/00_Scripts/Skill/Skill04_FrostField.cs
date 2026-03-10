using UnityEngine;

public class Skill04_FrostField : SkillBase
{
    Vector3 particleRange;
    float targetRange;

    private GameObject FrostFieldParticle;
    protected override void Fire()
    {
 
        var targets = targetLists(targetRange);

        foreach (var hit in targets)
        {
            hit.GetComponent<MONSTER>().GetDamage(Damage());
        }
        
   
    }

    protected override void OnInitalize()
    {
        float range = 1.2f + 0.1f * (level - 1);
        particleRange = Vector3.one * range;
        targetRange = 5.0f + 1.0f*(level - 1);

        //초기에 한번만 생성
        FrostFieldParticle = MANAGER.POOL.Pooling_OBJ("FrostField").Get((value) =>
        {
            //value.transform.parent = Player.instance.transform;
            //value.transform.localPosition = Vector3.zero;
            value.transform.localScale = particleRange;
        });
    }

    protected override void OnLevelUp()
    {
        float range = 1.2f + 0.1f * (level - 1);
        particleRange = Vector3.one * range;

        if(FrostFieldParticle != null)
        {
            FrostFieldParticle.transform.localScale = particleRange;
        }

        targetRange = 5.0f + 1.0f * (level - 1);
    }
}
