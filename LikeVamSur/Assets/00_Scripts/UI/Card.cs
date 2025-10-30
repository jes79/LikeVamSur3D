using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardDB card;

    [SerializeField] TextMeshProUGUI Title, Description;
    [SerializeField] Image IconImage;
    [SerializeField] Image OutlineImage;

    Animator animator;
    public Color[] colors;

    public bool isSelected = false;
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAnimation("Card_PointerDown");
    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetAnimation("Card_PointerUp");

    }

    private void Start()
    {
        
       animator = GetComponent<Animator>();    
    }

    public void Initialize(CardDB cardDB)
    {
        card = cardDB;

        Title.text = card.id;
        Description.text = string.Format(card.description, card.DamagePercentage);
        IconImage.sprite = MANAGER.DB.GetSprite(card.name);
        OutlineImage.color = card.state == CardState.Active ? colors[0] : colors[1];    
        animator.Rebind(); 
        isSelected = false;
    }

    public void SetAnimation(string temp)
    {
        if (isSelected)
        {
            return;
        }
        animator.Play(temp);
    }
}
