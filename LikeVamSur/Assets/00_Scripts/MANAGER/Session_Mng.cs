using System.Collections.Generic;
using UnityEngine;


public delegate void OnExpChanged(float exp);
public delegate void OnMonsterCountChanged(int value);
public delegate void OnSelectedCard();

public class Session_Mng : MonoBehaviour
{
    public OnExpChanged onExpChanged;
    public OnMonsterCountChanged onMonsterCountChanged;
    public OnSelectedCard onSelectedCard;

    public Dictionary<string , SelectCard> SelectedCards =  new Dictionary<string , SelectCard>(); 

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

    public void SelectedCard(CardDB db)
    {
        //CardSelector 에 GetCard(CardDB db)에서 해준걸 그냥 여기서 처리해 주겠음.
        if (SelectedCards.ContainsKey(db.id))
        {
            var data = SelectedCards[db.id];
            data.Level++;
        }
        else
        {
            var selected = new SelectCard();
            selected.db = db;
            selected.Level = 1;
            SelectedCards.Add(db.id, selected);
        }

        MANAGER.SKILL.RegisterSkill(db, SelectedCards[db.id].Level);
        Debug.Log(db.id + "카드가 선택되었습니다. \nLevel : " + SelectedCards[db.id].Level);
        onSelectedCard?.Invoke();

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
            //Time.timeScale = 0;
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
