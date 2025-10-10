using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public float lifetiem = 5.0f;
    public GameObject ExplosionParticle;

    public GameObject DamageObject;

    private Vector3 direction;



    public void Initialize(Vector3 dir)
    {
        direction = dir;
        //Destroy(this.gameObject, lifetiem);

        StartCoroutine(DestroyCoroutine(5));
    }

    IEnumerator DestroyCoroutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        MANAGER.POOL.m_pool_Dictionary["Projectile"].Return(this.gameObject);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Instantiate(ExplosionParticle, transform.position, Quaternion.identity);


            //GameObject damageFont = Instantiate(DamageObject);
            //damageFont.GetComponent<DamageTMP>().Initialize(
            //    Base_Canvas.instance.transform,
            //    transform.position,
            //    "10");

            /*
            var damageFont = MANAGER.POOL.Pooling_OBJ("DamageFont").Get((value) =>
            {
                value.GetComponent<DamageTMP>().Initialize(
                    Base_Canvas.instance.transform, 
                    transform.position, 
                    MANAGER.SESSION.Damage.ToString());
            });
            */
            other.gameObject.GetComponent<MONSTER>().GetDamage(MANAGER.SESSION.Damage);


            //Destroy(this.gameObject);
            MANAGER.POOL.m_pool_Dictionary["Projectile"].Return(this.gameObject);

        }
    }
}
