using UnityEngine;

public class Health_Effect : MonoBehaviour, IItemEffect
{
    public void OnPickUp(GameObject owner)
    {
        Debug.Log("HealthItem");
    }
}
