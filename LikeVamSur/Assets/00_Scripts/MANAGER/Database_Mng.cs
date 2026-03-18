using JetBrains.Annotations;
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
    public List<CardDB> GetRandomCardSet(bool AllActive = false)
    {
        /*
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
        */

        List<CardDB> result = new();

        List<CardDB> activeCandidates = new();
        List<CardDB> passiveCandidates = new();

        
        foreach(var card in ActiveCards)
        {
            if(CanBeSelected(card)) activeCandidates.Add(card);
        }

        foreach(var card in PassiveCards)
        {
            if(CanBeSelected(card)) passiveCandidates.Add(card);
        }

        if (AllActive)
        {
            int totalcount = Mathf.Min(3, activeCandidates.Count);

            while(result.Count < totalcount)
            {
                CardDB pick = activeCandidates[Random.Range(0, activeCandidates.Count)];

                if(!result.Contains(pick)) result.Add(pick);
            }

            return result.OrderBy(x=> Random.value).ToList();
        }

        if (activeCandidates.Count > 0)
        {
            result.Add(activeCandidates[Random.Range(0, activeCandidates.Count)]);
        }

        if(passiveCandidates.Count > 0)
        {
            result.Add(passiveCandidates[Random.Range(0, passiveCandidates.Count)]);
        }

        List<CardDB> candidates = new();
        candidates.AddRange(activeCandidates);
        candidates.AddRange(passiveCandidates);

        candidates.RemoveAll(x => result.Contains(x));

        int count = Mathf.Min(3, candidates.Count);

        while(result.Count < count)
        {
            CardDB pick = candidates[Random.Range(0, candidates.Count)];

            if(!result.Contains(pick)) result.Add(pick);
        }

        return result.OrderBy(x => Random.value).ToList();

    }

    private bool CanBeSelected(CardDB card)
    {
        var session = MANAGER.SESSION;
        if (session.SelectedCards.TryGetValue(card.id, out SelectCard selected))
        {
            return selected.Level < 5; 
        }

        int active = session.SelectedCards.Values.Count(x => x.db.state == CardState.Active);
        int passive = session.SelectedCards.Values.Count(x => x.db.state == CardState.Passive);

        if (card.state == CardState.Active && active >= 6) return false;    
        if(card.state == CardState.Passive && passive >= 6) return false;
        
        return true;
    }

    private PartDB GetDB(string path)
    {
        return Resources.Load<PartDB>("DB/Part/" + path);
    }
}
