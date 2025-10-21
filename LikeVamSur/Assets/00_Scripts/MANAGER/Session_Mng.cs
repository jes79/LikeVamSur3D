using UnityEngine;


public delegate void OnExpChanged(float exp);
public delegate void OnMonsterCountChanged(int value);
public class Session_Mng : MonoBehaviour
{
    public OnExpChanged onExpChanged;
    public OnMonsterCountChanged onMonsterCountChanged;

    public int CurrentWave;
    public int Level;
    public int Damage;
    public int monsterCount;

    public float magnetRadius = 3.0f;
    public float Exp;

    public float GameTime;

    public bool isGameOver = false;

    private void Update()
    {
        GameTime += Time.unscaledDeltaTime;//Time.timeScale = 0; 인상태에서도 작동됨.
    }
    public void AddMonster()
    {
        monsterCount++; 
        onMonsterCountChanged?.Invoke(monsterCount);
    }

    public void RemoveMonster()
    {
        monsterCount--;
        onMonsterCountChanged?.Invoke(monsterCount);
    }

    public void AddExp(float exp)
    {
        Exp += exp;
        if(Exp >= GetRequiredExp())
        {
            Exp = 0;
            Level++;
            Time.timeScale = 0;
            Base_Canvas.instance.SelectCard();
        }

        onExpChanged?.Invoke(Exp);
    }

    public int GetRequiredExp()
    {
        int level = Level + 1;
        //20, 40 level 에서는 레벨업 하기 어렵게..
        if (level < 20)
        {
            return (level * 10) - 5;
        }
        else if (level == 20)
        {
            return (level * 10) - 5 + 600;
        }
        else if (level < 40)
        {
            return (level * 13) - 6;
        }
        else if(level == 40)
        {
            return (level * 13) - 6 + 2400;
        }
        else
        {
            return (level * 16) - 8;
        }

    }
}
