using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    protected CardDB cardData;
    public string skillid;
    public float cooldown;
    public float timer;
    public int level;

    protected Transform target { get { return Player.instance.GetNearestMonster(); } }
    protected List<Transform> targetLists(float range)
    {
        return Player.instance.GetCollidersHitMonsters(range);
    }


    public void Initalize(CardDB data, int level)
    {
        cardData = data;
        skillid = cardData.id; //이렇게 안해주면 컴퍼넌트가 중복 추가됨.
        this.level = level;
        cooldown = cardData.baseCooldown - cardData.cooldownPerLevel*(level + 1);
        timer = 0.0f;
        OnInitalize();
    }

    public void LevelUp(int newLevel)
    {
        level = newLevel;
        cooldown = cardData.baseCooldown - cardData.cooldownPerLevel * (level + 1);
        OnLevelUp();
    }

    public void Tick()
    {
        timer += Time.deltaTime;
        if(timer > cooldown)
        {
            timer = 0.0f;
            Fire();
        }
    }

    protected float Damage()
    {
        float percent = cardData.baseDamage + cardData.damagePerLevel * (level-1);
        float baseDamage = MANAGER.SESSION.Damage;

        float finalDamge = baseDamage * (percent / 100f);
        return finalDamge;
    }

    protected void MakeBullet(Vector3 dir)
    {
       
        var bullet = MANAGER.POOL.Pooling_OBJ("Fireball").Get((value) =>
        {
            value.transform.position = Player.instance.transform.position + (Vector3.up*0.5f);
            value.transform.rotation = Quaternion.LookRotation(dir);
            value.GetComponent<Bullet>().Initialize(dir);
        });
    }

    protected abstract void OnInitalize();
    protected abstract void OnLevelUp();   
    protected abstract void Fire();


}
