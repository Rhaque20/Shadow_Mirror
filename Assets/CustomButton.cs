using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]Animator anim;
    private bool mouse_over = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouse_over = true;
        Debug.Log("Mouse enter "+this.name);
        anim.SetTrigger("Active");
        anim.SetInteger("state",1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouse_over = false;
        Debug.Log("Mouse exit "+this.name);
        anim.SetTrigger("Inactive");
        anim.SetInteger("state",0);
    }

    // public void OnPointerDown(PointerEventData eventData)
    // {
    //     Debug.Log("Clicked on this");
    // }
    
}
