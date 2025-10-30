using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class Database_Mng : MonoBehaviour
{
    public PartDB Monster;
    //public PartDB Projectile;

    //public CardDB Card;
    public List<CardDB> ActiveCards = new List<CardDB>();
    public List<CardDB> PassiveCards = new List<CardDB>();


    SpriteAtlas atlas;

    private void Start()
    {
        Monster = GetDB("Monster");
        //Projectile = GetDB("Projectile");
        atlas = Resources.Load<SpriteAtlas>("Atlas");

        //배열로 들어오는 값을 리스트로..
        //LoadAll 은 폴더 내부의 모든 객체를 다 참조함.
        ActiveCards = new List<CardDB>(Resources.LoadAll<CardDB>("DB/Card/Active"));
        PassiveCards = new List<CardDB>(Resources.LoadAll<CardDB>("DB/Card/Passive"));


    }

    public Sprite GetSprite(string temp)
    {
        return atlas.GetSprite(temp);   
    }

    //패시브, 액티브 카드를 랜덤하게 반환
    public List<CardDB> GetRandomCardSet()
    {
        List<CardDB> result = new List<CardDB>();

        var activeCard = ActiveCards[Random.Range(0, ActiveCards.Count)]; 
        result.Add(activeCard);

        var passiveCard = PassiveCards[Random.Range(0, PassiveCards.Count)];
        result.Add(passiveCard);


        bool pickActive = Random.value < 0.5f;
        var thirdPool = pickActive ? ActiveCards : PassiveCards;

        CardDB thirdCard = null;


        do
        {
            thirdCard = thirdPool[Random.Range(0, thirdPool.Count)];
        }
        while (result.Contains(thirdCard) && thirdPool.Count > 1);
       
        result.Add(thirdCard);

        return result.OrderBy(x => Random.value).ToList();
    }

    private PartDB GetDB(string path)
    {
        return Resources.Load<PartDB>("DB/Part/" + path);
    }
}
