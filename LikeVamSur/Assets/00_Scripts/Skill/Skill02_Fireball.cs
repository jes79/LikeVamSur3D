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

        if(count == 1)
        {
            Vector3 dir = Player.instance.transform.forward;
            MakeBullet(dir);
        }
        else
        {
            float step = spread / (count - 1);//count가 1이면 분모가 0이 되서 오류
            float start = -spread / 2f;

            for (int i = 0; i < count; i++)
            {
                float angle = start + (step * i);
                Vector3 dir = Quaternion.Euler(0, angle, 0) * Player.instance.transform.forward;

                MakeBullet(dir);
            }
        }
          
    }


}
