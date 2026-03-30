using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure_Card : MonoBehaviour
{
    public RectTransform rollerParent;
    public GameObject cardPrefab;
    public float cardHeight = 240f;
    private float rollDuration;
    private int loopCount;

    private List<RectTransform> cards = new();

   
    private void Start()
    {
        //테스트 
        //얻은 카드도 없고 보스몬스터도 없기 때문에 임시로..
        List<CardDB> cards = new List<CardDB> ();
        CardDB card01 = new CardDB ();
        CardDB card02 = new CardDB ();  
        CardDB card03 = new CardDB ();  
        CardDB card04 = new CardDB ();  
        CardDB card05 = new CardDB ();  

        cards.Add (card01);
        cards.Add (card02);
        cards.Add (card03);
        cards.Add (card04);
        cards.Add (card05);

        Initialized(cards);

    }
    public void Initialized(List<CardDB> candidateCards)
    {
        rollDuration = Random.Range(5f, 8f);
        loopCount = Random.Range(5, 10);

        foreach(Transform child in rollerParent)
        {
            Destroy(child.gameObject);
        }
        cards.Clear();  

        for(int i = 0;  i < candidateCards.Count; i++)
        {
            var pick = candidateCards[i];
            GameObject go = Instantiate(cardPrefab, rollerParent);
            go.SetActive(true);
            RectTransform rt = go.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(0, -i*cardHeight);
            cards.Add(rt);
        }

        rollerParent.anchoredPosition = Vector2.zero;

        StartCoroutine(RollingCoroutine());
    }

    IEnumerator RollingCoroutine()
    {
        int targetIndex = Random.Range(0, cards.Count);
        int totalSteps = loopCount*cards.Count + targetIndex;
        float totalDistance = totalSteps * cardHeight;

        Vector2 startPos = rollerParent.anchoredPosition;
        Vector2 endPos = startPos - new Vector2(0, totalDistance);
        float elapsed = 0f;

        while (elapsed < rollDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = EaseOutQuart(elapsed / rollDuration);
            rollerParent.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].anchoredPosition.y + rollerParent.anchoredPosition.y < -cardHeight)
                {
                    float highestY = GetHighestCardY();
                    cards[i].anchoredPosition = new Vector2(0, highestY + cardHeight);
                }
            }
            yield return null;
        }

        RectTransform selectedCard = cards[targetIndex];
        float offset = selectedCard.anchoredPosition.y + rollerParent.anchoredPosition.y;
        rollerParent.anchoredPosition -= new Vector2(0, offset);
    }

    float GetHighestCardY()
    {
        float maxY = float.MinValue;
        foreach(var card in cards)
        {
            if(card.anchoredPosition.y > maxY)
            {
                maxY = card.anchoredPosition.y;
            }
        }
        return maxY;    
    }

    private float EaseOutQuart(float t)
    {
        return 1 - Mathf.Pow(1 - t, 4);
    }
}
