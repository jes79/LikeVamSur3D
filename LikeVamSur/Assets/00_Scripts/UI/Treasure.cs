using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Treasure : MonoBehaviour
{
    [SerializeField] Image ChestImage;
    [SerializeField] Sprite[] ChestSprites;
    [SerializeField] Treasure_Card[] cards;

    [SerializeField] GameObject ConfirmBtn;

    int valueCount = 0;
    Animator animator;

    CanvasGroup canvasGroup;


    private void CanvasGroupCheck(bool B)
    {
        canvasGroup.interactable = B;
        canvasGroup.blocksRaycasts = B;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void Initialize(int chestValue)
    {
        //List<SelectCard> lists = new List<SelectCard>();

        //foreach (var selected in MANAGER.SESSION.SelectedCards)
        //{
        //    if(selected.Value.Level < 5)
        //    {
        //        lists.Add(selected.Value);
        //    }
        //}

        CanvasGroupCheck(true);


        animator.Play("Selector_Open");
        ChestImage.sprite = ChestSprites[chestValue];
        valueCount = chestValue;

        switch (chestValue)
        { 
            case 0:  //브론즈
                cards[0].gameObject.SetActive(true);
                cards[0].Initialized(lists());
                break;
            case 1: //실버
                for(int i = 0; i < 3; i++)
                {
                    cards[i].gameObject.SetActive(true);
                    cards[i].Initialized(lists());
                }
                    
                break;
            case 2: //골드
                for(int i = 0; i < 5; i++)
                {
                    cards[i].gameObject.SetActive(true);
                    cards[i].Initialized(lists());
                }                
                break;

        }
    }

    public List<SelectCard> lists()
    {
        List<SelectCard> lists = new List<SelectCard>();

        foreach (var selected in MANAGER.SESSION.SelectedCards)
        {
            if (selected.Value.Level < 5)
            {
                lists.Add(selected.Value);
            }
        }

        if (lists.Count == 0)
        {
            for(int i = 0; i < MANAGER.DB.NoneCards.Count; i++)
            {
                lists.Add(new SelectCard
                {
                    db = MANAGER.DB.NoneCards[i],
                    Level = 0
                });
            }
        }
        return lists;
    }

    public void ConfirmCheck()
    {
        switch (valueCount)
        {
            case 0:  //브론즈
                if(!cards[0].isFinished) return;
                break;
            case 1: //실버
                for (int i = 0; i < 3; i++)
                    if (!cards[0].isFinished) return;
                break;
            case 2: //골드
                for (int i = 0; i < 5; i++)
                    if (!cards[0].isFinished) return;
                break;

        }

        ConfirmBtn.transform.localScale = Vector3.one;

    }

    public void Confirm()
    {
        Time.timeScale = 1f;
        //ConfirmBtn.SetActive(false);
        for(int i = 0; i< cards.Length; i++)
        {
            cards[i].gameObject.SetActive(false);
        }

        CanvasGroupCheck(false);
        ConfirmBtn.transform.localScale = Vector3.zero;
        animator.Play("Selector_Close");

        
       
    }
}
