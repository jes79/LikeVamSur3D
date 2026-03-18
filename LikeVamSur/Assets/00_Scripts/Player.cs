using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Player : MonoBehaviour
{
    public static Player instance;

    public float detectionRadius = 10.0f;
    public LayerMask monsterLayer;
    public List<Transform> targets = new List<Transform>();  
    public Renderer[] renderer;
    bool isHIt = false;
    public Transform target { get { return GetNearestMonster(); } }

    [SerializeField] private Volume volume;
    private Vignette vignette;
    [SerializeField] private Color vignteeColor;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        volume.profile.TryGet(out vignette);
    }

    IEnumerator VignettPulse()
    {
        float t = 0f;
        float duration = 0.2f;//0.15f;
        float maxIntensity = 0.4f;//0.4f;

        vignette.color.value = vignteeColor;

        while(t < duration)
        {
            t += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(0f, maxIntensity, t / duration);
            yield return null;
        }

        t = 0f;
        while(t < duration)
        {
            t += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(maxIntensity, 0f, t / duration);
            yield return null;
        }

        vignette.intensity.value = 0f;
    }


    IEnumerator CameraShake(Transform camTransform, float duration, float strength)
    {
        Vector3 orignalPos = camTransform.localPosition;
        float timer = 0f;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            float offsetX = Random.Range(-1f, 1f)*strength;
            float offsetY = Random.Range(-1f, 1f)* strength;

            camTransform.localPosition = orignalPos + new Vector3(offsetX, offsetY, 0f);
            yield return null;
        }

        camTransform.localPosition = orignalPos;
    }
    public Vector3 Direction()
    {
        Vector3 dirToMonster = (target.position - transform.position).normalized;
        return dirToMonster;
    }

    public List<Transform> GetCollidersHitMonsters(float radius)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, monsterLayer);
        List<Transform> targetLists = new List<Transform>();

        foreach (Collider col in hits)
        {
            if (col.GetComponent<MONSTER>().isSpawned)
            {
                targetLists.Add(col.transform);
            }
        }
        return targetLists;
    }
    public Transform GetNearestMonster()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayer);
        Transform nearest = null;
        float minDist = Mathf.Infinity;
        targets = new List<Transform>();

        foreach (Collider col in hits)
        {
            if (col.GetComponent<MONSTER>().isSpawned)
            {
                targets.Add(col.transform);
                float dist = Vector3.Distance(transform.position, col.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = col.transform;
                }
            }
        }

        return nearest;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("Check");
            if (isHIt == false)
            {
                GetDamage(10);
            }
            
        }
    }

    public void GetDamage(float dmg)
    {
        isHIt = true;
        StartCoroutine(FlashEmission(0.5f));
        StartCoroutine(CameraShake(Camera.main.transform, 0.2f, 0.1f));

        StartCoroutine(VignettPulse());

        var damageFont = MANAGER.POOL.Pooling_OBJ("DamageFont").Get((value) =>
        {
            value.GetComponent<DamageTMP>().Initialize(
            Base_Canvas.instance.HOLDERLAYER,
            transform.position,
            ((int)dmg).ToString(),
            Color.red
            ); // MANAGER.SESSION.Damage -> dmg
        });


        MANAGER.SESSION.GetDamage(dmg);

    }

    IEnumerator FlashEmission(float fadeTime)
    {
        Color flashColor = Color.white * 4f;

        float timer = 0f;
        while(timer < fadeTime)
        {
            timer += Time.deltaTime;
            Color current = Color.Lerp(flashColor, Color.black, timer / fadeTime);
             
            for (int i = 0; i < renderer.Length; i++)
            {
                renderer[i].material.SetColor("_EmissionColor", current);
            }
            yield return null;
        }

        for (int i = 0; i < renderer.Length; i++)
        {
            renderer[i].material.SetColor("_EmissionColor", Color.black);
        }

        isHIt = false;
    }
}
