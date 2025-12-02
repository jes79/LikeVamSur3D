using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public float detectionRadius = 10.0f;
    public LayerMask monsterLayer;
    public List<Transform> targets = new List<Transform>();  
    public Transform target { get { return GetNearestMonster(); } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public Vector3 Direction()
    {
        Vector3 dirToMonster = (target.position - transform.position).normalized;
        return dirToMonster;
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
        }//

        return nearest;

    }
}
