using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemIcon : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler, IPointerUpHandler, IPointerClickHandler,
    IBeginDragHandler,IDragHandler,IEndDragHandler
{
    Vector2 dragOffset = Vector2.zero;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("OnPointerEnter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnPointerExit");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("OnPointerUp");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("OnPointerClick");
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Image>().raycastTarget = false;
        dragOffset = (Vector2)transform.position - eventData.position;
        transform.parent.SetAsLastSibling();
    }
    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position + dragOffset;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        GetComponent<Image>().raycastTarget = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
