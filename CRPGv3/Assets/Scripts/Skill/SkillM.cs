using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillM : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public SkillData orgData;
    Vector2 dragOffset = Vector2.zero;
    public GameObject selectedSkill;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 pos = Input.mousePosition;
        selectedSkill = Instantiate(Resources.Load("Prefabs/SelectedSkill"), pos, Quaternion.identity, transform.parent.parent.parent.parent) as GameObject;
        selectedSkill.GetComponent<SelectedSkill>().myData = orgData;
        dragOffset = (Vector2)selectedSkill.transform.localPosition - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        selectedSkill.transform.localPosition = eventData.position + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(selectedSkill);
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
