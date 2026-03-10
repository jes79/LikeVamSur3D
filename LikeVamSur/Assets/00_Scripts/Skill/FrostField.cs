using UnityEngine;

public class FrostField : MonoBehaviour
{
    private void Update()
    {
        transform.position = Player.instance.transform.position;
    }
}
