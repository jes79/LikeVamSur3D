using UnityEngine;


public delegate void OnExpChanged(float exp);
public class Session_Mng : MonoBehaviour
{
    public OnExpChanged onExpChanged;
    public int CurrentWave;
    public int Level;
    public int Damage;

    public float magnetRadius = 3.0f;
    public float Exp;

    public float GameTime;

    public bool isGameOver = false;


    public void AddExp(float exp)
    {
        Exp += exp;
        if(Exp >= 100)
        {
            Exp = 0;
            Level++;
        }

        onExpChanged?.Invoke(Exp);
    }
}
