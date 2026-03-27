using UnityEngine;

public class Magnet_Effect : MonoBehaviour, IItemEffect
{
    public void OnPickUp(GameObject owner)
    {
        foreach(var orb in MANAGER.SESSION.Orbs)
        {
            orb.StartFollow(Player.instance.transform); 
        }
    }
}
