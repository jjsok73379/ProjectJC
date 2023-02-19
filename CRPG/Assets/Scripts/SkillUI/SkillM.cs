using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillM : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public SkillData myData;
    Vector2 dragOffset = Vector2.zero;
    public GameObject selectedSkill;
    public GameObject myReinfoce;
    ReinforceSkill theReinforceSkill;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 pos = Input.mousePosition;
        selectedSkill = Instantiate(Resources.Load("Prefabs/SelectedSkill"), pos, Quaternion.identity, transform.parent.parent.parent.parent) as GameObject;
        selectedSkill.GetComponent<SelectedSkill>().myData = myData;
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
        theReinforceSkill = ReinforceSkill.Inst;
        myReinfoce.GetComponent<Button>().onClick.AddListener(SkillManager.Inst.OpenReinFoce);
    }

    // Update is called once per frame
    void Update()
    {
        ShowReinforceButton();
    }

    public void ShowReinforceButton()
    {
        for (int i = 0; i < SkillManager.Inst.CombinedSkills.Count; i++)
        {
            if (SkillManager.Inst.CombinedSkills.Contains(myData))
            {
                myReinfoce.SetActive(true);
                theReinforceSkill.ReinfoceData = myData;
            }
            else
            {
                myReinfoce.SetActive(false);
            }
        }
    }
}
