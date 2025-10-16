using UnityEngine;

public class Player_Detector : MonoBehaviour
{
    //public float magnetRadius = 3.0f;
    public LayerMask orbLayer;

    private void Update()
    {
        //Collider[] hits = Physics.OverlapSphere(transform.position, magnetRadius, orbLayer);
        Collider[] hits = Physics.OverlapSphere(transform.position, 
                                                MANAGER.SESSION.magnetRadius, orbLayer);
        foreach (var hit in hits)
        {
            Orb orb = hit.GetComponent<Orb>();
            if (orb != null)
            {
                orb.StartFollow(transform);
            }
        }
    }
}
