using UnityEngine;

public class Magnet_Effect : MonoBehaviour, IItemEffect
{

    bool isPickUp = false;
    public void Initialize()
    {
        isPickUp = false;
    }

    public void OnPickUp(GameObject owner)
    {
        if (isPickUp) return;
        isPickUp = true;

        foreach(var orb in MANAGER.SESSION.Orbs)
        {
            orb.StartFollow(Player.instance.transform); 
        }
    }
}
