using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        print("Enter"); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        print("Exit"); 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("Down"); 
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        print("Up"); 
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Click"); 
    }

   
}
