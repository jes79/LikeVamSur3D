using UnityEngine;

public class MONSTER : MonoBehaviour
{
    public float HP;
    public float MaxHP;

    public Transform target;
    public string monsterid;

    public bool isDead = false;
    //protected bool isSpawned = false;
    public bool isSpawned = false;

    private IFactory<MONSTER> factory;

    public virtual void Initialize(Transform player)
    {
        MANAGER.SESSION.AddMonster();   
        isSpawned = false;
        //임시로..
        HP = 10;
        MaxHP = HP;

        isDead = false;
        monsterid = Random.Range(0, 2) == 1 ? "Skeleton_01" : "Skeleton_02";
        factory = new GenericPartFactory<MONSTER>(MANAGER.DB.Monster);
        target = player;

        factory.Build(this, monsterid);
    }

    public void GetDamage(float dmg)
    {
        HP -= dmg;

        var damageFont = MANAGER.POOL.Pooling_OBJ("DamageFont").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initialize(
        Base_Canvas.instance.HOLDERLAYER,
        transform.position,
        ((int)dmg).ToString()); // MANAGER.SESSION.Damage -> dmg
        });

        if (HP <= 0)
        {
            isDead = true;

            MANAGER.SESSION.RemoveMonster();
            //Debug.Log("사망");
            var deadEffect = MANAGER.POOL.Pooling_OBJ("DeadEffect").Get((value) =>
            {
                value.transform.position = transform.position;
            });


            MANAGER.instance.Run(Util_Coroutine.Delay(
                0.5f,
                () => MANAGER.POOL.m_pool_Dictionary["DeadEffect"].Return(deadEffect)));


            //익펙트가 가지고 있는 Duration 값(0.5)만큼 지연
            //딜레이 후 실행 할 동작은 액션으로 전달
            /*
            StartCoroutine(Util_Coroutine.Delay(
                0.5f,
                () => MANAGER.POOL.m_pool_Dictionary["DeadEffect"].Return(deadEffect)));
            */
            MANAGER.POOL.m_pool_Dictionary["Monster"].Return(this.gameObject);

            //DropEXP(transform.position, Random.Range(1.0f, 5.0f));
            DropEXP(transform.position, Random.Range(10.0f, 50.0f));
        }
    }

    private void DropEXP(Vector3 deathPostion, float exp = 1.0f)
    {
        //float[] units = { 3.0f, 1.0f, 0.25f};
        float[] units = { 50.0f, 30.0f, 10.0f };

        foreach (float unit in units)
        {
            while(exp >= unit)
            {
                exp -= unit;
                OrbMake(deathPostion, unit);

            }


        }

        if (exp > 0.01f)
        {
            OrbMake(deathPostion, exp);
        }

    }

    private void OrbMake(Vector3 deathPosition, float exp)
    {
        Vector3 spawnPos = deathPosition + Utils_World.GetRandomCircleOffset(1.5f);
        spawnPos.y += 0.5f;
        var orb = MANAGER.POOL.Pooling_OBJ("Orb").Get((value) =>
        {
            //value.transform.position = spawnPos;
            value.transform.position = transform.position;
            value.GetComponent<Orb>().Initialize(exp, spawnPos);
        });
    }

}
