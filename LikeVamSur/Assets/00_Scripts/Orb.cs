using System.Collections;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public float expValue;
    public Color[] colors;
    Renderer renderer;
    public bool isIdle = false;
    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }
    public void Initialize(float amount, Vector3 endPosition)
    {
        expValue = amount;
        DropExp(endPosition);   
        if (amount == 3f)
        {
            transform.localScale = Vector3.one * 0.5f;
            renderer.material.color = colors[0];
        }
        else if(amount == 1.0f)
        {
            transform.localScale = Vector3.one * 0.4f;
            renderer.material.color = colors[1];
        }
        else if (amount == 0.25f)
        {
            transform.localScale = Vector3.one * 0.3f;
            renderer.material.color = colors[2];
        }
        else
        {
            transform.localScale = Vector3.one * 0.2f;
            renderer.material.color = colors[3];
        }
    }

    public void DropExp(Vector3 end)
    {
        float height = Random.Range(1.0f, 2.0f);
        float duration = Random.Range(0.3f, 0.5f);
        

        StartCoroutine(Util_Coroutine.ParabolaMove(transform,
                                                   transform.position,
                                                   end,
                                                   height,
                                                   duration,
                                                   ()=> isIdle = true));
    }


    public void StartFollow(Transform target)
    {
        if(!isIdle) return;
        StartCoroutine(MoveToPlayer(target));   
    } 


    //플레이어의 반대방향으로 날아갔다가 다시 플레이어에게 날아가는 연출
    IEnumerator MoveToPlayer(Transform player)
    {
        isIdle = false;

        Vector3 ejectDir = (transform.position - player.position).normalized;
        float ejectTime = 0.15f;
        float ejectSpeed = 4.0f;
        float timer = 0.0f;

        while(timer < ejectTime)
        {
            transform.position += ejectDir * ejectSpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;  
        }

        float absorbSpeed = 10f;
        while (true)
        {
            Vector3 endPos = player.position + new Vector3(0, 0.5f, 0);
            transform.position = Vector3.MoveTowards(transform.position,
                                                     endPos, 
                                                     absorbSpeed*Time.deltaTime);

            float dist = Vector3.Distance(transform.position, endPos);

            if(dist < 0.2) break;
            yield return null;
        }
        Absorb();
    }

    void Absorb()
    {
        MANAGER.POOL.m_pool_Dictionary["Orb"].Return(this.gameObject);
        MANAGER.SESSION.AddExp(expValue);
    }
}
