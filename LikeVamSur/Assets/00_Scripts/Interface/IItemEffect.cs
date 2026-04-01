using UnityEngine;

public interface IItemEffect 
{
    void Initialize();
    void OnPickUp(GameObject owner);
}
