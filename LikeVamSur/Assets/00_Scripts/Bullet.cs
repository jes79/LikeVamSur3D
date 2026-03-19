using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Effect_Status status;
    public float Damage;
    public string BulletName;
    public float speed = 10.0f;
    public float lifetiem = 5.0f;
    public float delay;
    private Vector3 direction;

    bool isHit = false;
    [SerializeField] private ParticleSystem BulletParticle;
    [SerializeField] private GameObject ExplosionParticle;
    

    public void Initialize(Vector3 dir, float dmg, Effect_Status status = Effect_Status.None)
    {
        isHit = false;
        this.status = status; 
        Damage = dmg;
        direction = dir;
        //Destroy(this.gameObject, lifetiem);
        BulletParticle.gameObject.SetActive(true);
        BulletParticle.Clear(); //기존 입자 제거
        BulletParticle.Play(); //다시 실행
        ExplosionParticle.SetActive(false);
        StartCoroutine(DestroyCoroutine(5));
    }

    IEnumerator DestroyCoroutine(float timer)
    {
        yield return new WaitForSeconds(timer);
        MANAGER.POOL.m_pool_Dictionary[BulletName].Return(this.gameObject);
    }

    void Update()
    {
        if (isHit) return;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHit) return;
        if (other.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
           isHit = true;
            BulletParticle.gameObject.SetActive(false );   
            ExplosionParticle.SetActive(true);


            other.gameObject.GetComponent<MONSTER>().GetDamage(Damage);

            switch (status)
            {
                case Effect_Status.None:
                    break;
                case Effect_Status.Burn:
                    other.gameObject.GetComponent<StatusEffect>().ApplyBurn();
                    break;
            }

            StopAllCoroutines();
            StartCoroutine(WaitEffectAndReturn(delay));
        }
      
    }
    IEnumerator WaitEffectAndReturn(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnBullet();
    }

    private void ReturnBullet()
    {
        ExplosionParticle.SetActive(false); 
        StopAllCoroutines();
        MANAGER.POOL.m_pool_Dictionary[BulletName].Return(this.gameObject);
    }
}