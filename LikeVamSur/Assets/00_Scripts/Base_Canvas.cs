using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance  = null;

    private void Awake()
    {
        if(instance == null) {instance = this;} 
    }
}
