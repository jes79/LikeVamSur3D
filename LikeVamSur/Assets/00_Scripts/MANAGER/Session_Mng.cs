using System.Collections.Generic;
using UnityEngine;


public delegate void OnExpChanged(float exp);
public delegate void OnMonsterCountChanged(int value);
public delegate void OnSelectedCard();
public delegate void OnHpChanged(float hp);


public class Session_Mng : MonoBehaviour
{
    public OnExpChanged onExpChanged;
    public OnMonsterCountChanged onMonsterCountChanged;
    public OnSelectedCard onSelectedCard;
    public OnHpChanged onHpChanged;

    public Dictionary<string , SelectCard> SelectedCards =  new Dictionary<string , SelectCard>(); 

    public int CurrentWave;
    public int Level;

    public int monsterCount;
    public float Exp;

    public float GameTime;

    public float baseMaxHP;
<<<<<<< HEAD
    public float baseDamage;
=======
>>>>>>> def53b9079a1719a102236a5f6286b0906b1348a

    [Space(20f)]
    [Header("## Player Data ##")]
    public float HP;
<<<<<<< HEAD
    public float Damage => baseDamage * (1f + DamagePercent / 100f);
=======
>>>>>>> def53b9079a1719a102236a5f6286b0906b1348a
    public float MaxHP => baseMaxHP*(1f + HPPercent/100f);
    public float magnetRadius;

    [Space(20f)]
    [Header("## Player Plus Data ##")]
    public float DamagePercent;
    public float HPPercent;
    public float magnetRadiusPercent;
    public float expPlusPercent;
    public float CriticalPercent;
    public float CriticalDamagePercent;
 




    public bool isGameOver = false;


    private void Start()
    {
        //MaxHP = HP;
        baseMaxHP = HP;

        Base_Canvas.instance.HPChanged(HP);

    }
    private void Update()
    {
        GameTime += Time.unscaledDeltaTime;//Time.timeScale = 0; 인상태에서도 작동됨.
    }

    public void RefreshHpbyPercent(float oldMaxHP)
    {
        //비율로 증가하게. 100 : 100 = 120 : 120
        //90 : 100 = x : 120 -> 100x = 120*90 -> x = (120*90)/100 -> x = 108
        float ratio = HP / oldMaxHP; // 90/100 - > 0.9
        HP = MaxHP * ratio; //120*0.9 = 108  

        onHpChanged?.Invoke(HP);
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

    public void GetDamage(float dmg)
    {
        HP -= dmg;
        onHpChanged?.Invoke(HP);
    }

    public void AddExp(float exp)
    {
        float realExp = exp + exp * (expPlusPercent / 100);
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

    public bool GetCritical()
    {
        float RandomValue = Random.value * 100f;
        if(RandomValue <= CriticalPercent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
