using System.Collections;
using UnityEngine;

public class Player_Attacker : MonoBehaviour
{
    public GameObject bulletPrefab;


    private void Start()
    {
        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        yield return new WaitForSeconds(1.0f);
        FireProjectile();
        StartCoroutine(FireCoroutine()); //재귀 반복되야 해서 (InvokeRepeating)

    }
    void FireProjectile()
  {
        Vector3 fireDir;
        if (Player.instance.target != null)
        {
            //fireDir = (Player.instance.target.position - transform.position).normalized;
            fireDir = Player.instance.Direction();
        }
        else 
        { 
            fireDir = transform.forward;
        }


        //GameObject bullet = Instantiate(bulletPrefab, (transform.position+ new Vector3(0,1f,0)) + fireDir, Quaternion.identity);
        //bullet.GetComponent<Bullet>().Initialize(fireDir);

        var bullet = MANAGER.POOL.Pooling_OBJ("Projectile").Get((value) =>
        {
            Vector3 pos = transform.position + new Vector3(0, 1.0f, 0.0f) + fireDir*1.0f;
            value.transform.position = pos;
            value.GetComponent<Bullet>().Initialize(fireDir);
        });
  }
}
