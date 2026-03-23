using UnityEngine;

public class Monster_Renderer_Finder : MonoBehaviour
{
    public Renderer renderer;

    public void Initialize()
    {
        StatusEffect effect = transform.parent.GetComponent<StatusEffect>();
        effect.renderer = this.renderer;
    }
}
