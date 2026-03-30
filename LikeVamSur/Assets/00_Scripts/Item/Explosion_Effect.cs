using UnityEngine;

public class Explosion_Effect : MonoBehaviour, IItemEffect
{
    public float radius = 8f;
    public float damage = 100f;

    public GameObject ExplosionEffect;

    public void OnPickUp(GameObject owner)
    {
        Vector3 center = transform.position + Vector3.up*3f;
        Collider[] hits = Physics.OverlapSphere(center, radius, LayerMask.GetMask("Monster"));

        foreach(var hit in hits)
        {
            if(hit.TryGetComponent(out MONSTER m) && m.isSpawned)
            {
                m.GetDamage(damage);
            }
        }

        Instantiate(ExplosionEffect, center, Quaternion.identity);
    }
}
