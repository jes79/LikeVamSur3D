using System.Collections;
using UnityEngine;

public class CardSelector : MonoBehaviour
{
    public Card[] cards;  
    Animator animator;
    bool isSelected = false;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Initialize(bool AllActive = false)
    {
        isSelected = false;
        animator.Play("Selector_Open");

        var Cards = MANAGER.DB.GetRandomCardSet(AllActive);
        for(int i = 0; i < cards.Length; i++) 
        {
            cards[i].Initialize(Cards[i]);
        }
    }

    public void SelectCard(int value)
    {
        if(isSelected) return;
        isSelected = true;
        for (int i = 0; i < cards.Length; i++)
        {
            
            if (i == value)
            {
                cards[i].SetAnimation("Card_Select");
                //GetCard(cards[i].card);
                MANAGER.SESSION.SelectedCard(cards[i].card);

            }
            else
            {
                cards[i].SetAnimation("Card_NoneSelect");
            }
            cards[i].isSelected = true; 
        }
        StartCoroutine(GameStartCoroutine());
    }

    /*(미사용 불필요)
    public void GetCard(CardDB db)
    {
        //
    }
    */


    IEnumerator GameStartCoroutine()
    {
        yield return new WaitForSecondsRealtime(1.0f);
        animator.Play("Selector_Close");
        Time.timeScale = 1.0f;
        //test
        //Base_Canvas.instance.SetSkillFrame();
    }
}
