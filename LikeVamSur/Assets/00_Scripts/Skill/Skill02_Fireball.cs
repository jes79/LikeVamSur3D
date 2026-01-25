using UnityEngine;

public class Skill02_Fireball : SkillBase
{
    int count;
    
    protected override void OnInitalize() 
    {
        count = 1 + 2 * (level - 1);
    }


    protected override void OnLevelUp() 
    {
        count = 1+2*(level - 1);
    }


    protected override void Fire()
    {
        float spread = 45f;
        float step = spread /(count - 1);
        float start = -spread / 2f;

        for(int i = 0; i< count; i++)
        {
            float angle = start + (step * i);
            Vector3 dir = Quaternion.Euler(0, angle, 0)*Player.instance.transform.forward;

            var bullet = MANAGER.POOL.Pooling_OBJ("Fireball").Get((value) =>
            {
                value.transform.position = Player.instance.transform.position;
                value.transform.rotation = Quaternion.LookRotation(dir);
                value.GetComponent<Bullet>().Initialize(dir);
            });
        }
    }
}
