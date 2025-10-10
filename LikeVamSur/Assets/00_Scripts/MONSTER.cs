using UnityEngine;

public class MONSTER : MonoBehaviour
{
    public int HP;
    public int MaxHP;

    public Transform target;
    public string monsterid;

    public bool isDead = false;
    protected bool isSpawned = false;

    private IFactory<MONSTER> factory;

    public virtual void Initialize(Transform player)
    {
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

    public void GetDamage(int dmg)
    {
        HP -= dmg;

        var damageFont = MANAGER.POOL.Pooling_OBJ("DamageFont").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initialize(
        Base_Canvas.instance.transform,
        transform.position,
        dmg.ToString()); // MANAGER.SESSION.Damage -> dmg
        });

        if (HP <= 0)
        {
            isDead = true;
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
        }


    }
}
