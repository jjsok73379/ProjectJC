using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveBar : MonoBehaviour,IBeginDragHandler,IDragHandler
{
    Vector2 dragOffset = Vector2.zero;
    public GameObject myUI;
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragOffset = (Vector2)myUI.transform.position - eventData.position;
        myUI.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        myUI.transform.position = eventData.position + dragOffset;
    }
}
