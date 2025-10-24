using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Animator animator;
    //
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.Play("Card_PointerDown");
    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        animator.Play("Card_PointerUp");

    }

    private void Start()
    {
        
       animator = GetComponent<Animator>();    
    }
}
