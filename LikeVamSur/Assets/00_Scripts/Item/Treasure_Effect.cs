using UnityEngine;

public class Treasure_Effect : MonoBehaviour, IItemEffect
{
    public ParticleSystemRenderer particle;

    public Material[] materials;

    int value = 0;
    public bool isPickUp = false;

    public void Initialize()
    {
        isPickUp = false;
        int randomValue = Random.Range(0, 3);
        value = randomValue;
        particle.material = materials[randomValue];
    }

    private void Start()
    {
        //int randomValue = Random.Range(0, 3);
        //value = randomValue;
        //particle.material = materials[randomValue];
    }

    public void OnPickUp(GameObject owner)
    {
        if (isPickUp) return;   
        isPickUp = true;
        Base_Canvas.instance.SelectTreasure(value);
    }

  
}
