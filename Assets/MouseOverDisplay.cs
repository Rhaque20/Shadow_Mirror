using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MouseOverDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // if (!EventSystem.current.IsPointerOverGameObject())
            // {
            //     Debug.Log("Clicked out of Bounds");
            // }
            // else
            // {
                
            // }
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    if (hit.collider != null)
                    {
                        Debug.Log("Clicked on "+hit.collider.gameObject.name);
                    }
                }
        }
    }
}
