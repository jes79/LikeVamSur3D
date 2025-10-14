using UnityEngine;

public class Orb : MonoBehaviour
{
    public float expValue;
    public Color[] colors;
    Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }
    public void Initialize(float amount, Vector3 endPosition)
    {
        expValue = amount;
        DropExp(endPosition);   
        if (amount == 3f)
        {
            transform.localScale = Vector3.one * 0.5f;
            renderer.material.color = colors[0];
        }
        else if(amount == 1.0f)
        {
            transform.localScale = Vector3.one * 0.4f;
            renderer.material.color = colors[1];
        }
        else if (amount == 0.25f)
        {
            transform.localScale = Vector3.one * 0.3f;
            renderer.material.color = colors[2];
        }
        else
        {
            transform.localScale = Vector3.one * 0.2f;
            renderer.material.color = colors[3];
        }
    }

    public void DropExp(Vector3 end)
    {
        float height = Random.Range(1.0f, 2.0f);
        float duration = Random.Range(0.3f, 0.5f);
        

        StartCoroutine(Util_Coroutine.ParabolaMove(transform,
                                                   transform.position,
                                                   end,
                                                   height,
                                                   duration));
    }
}
