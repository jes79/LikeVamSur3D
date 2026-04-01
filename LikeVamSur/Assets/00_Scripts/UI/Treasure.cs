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

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Initialize(int chestValue)
    {
        List<SelectCard> lists = new List<SelectCard>();

        foreach (var selected in MANAGER.SESSION.SelectedCards)
        {
            if(selected.Value.Level < 5)
            {
                lists.Add(selected.Value);
            }
        }
        
        animator.Play("Selector_Open");
        ChestImage.sprite = ChestSprites[chestValue];
        valueCount = chestValue;

        switch (chestValue)
        { 
            case 0:  //브론즈
                cards[0].gameObject.SetActive(true);
                cards[0].Initialized(lists);
                break;
            case 1: //실버
                for(int i = 0; i < 3; i++)
                {
                    cards[i].gameObject.SetActive(true);
                    cards[i].Initialized(lists);
                }
                    
                break;
            case 2: //골드
                for(int i = 0; i < 5; i++)
                {
                    cards[i].gameObject.SetActive(true);
                    cards[i].Initialized(lists);
                }
                    
                break;

        }
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

        animator.Play("Selector_Close");

        ConfirmBtn.transform.localScale = Vector3.zero;

    }
}
