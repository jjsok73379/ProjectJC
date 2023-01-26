using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour,IDropHandler
{
    public Image myImage;
    public SkillData mySkillData;
    Coroutine act = null;
    public GameObject myMenu = null;
    public GameObject mySlot = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mySlot.name == "SkillQ")
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
        if (mySlot.name == "SkillW")
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
        if (mySlot.name == "SkillE")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
        if (mySlot.name == "SkillR")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject icon = eventData.pointerDrag.GetComponent<SkillM>().selectedSkill;
        Image[] skillchild = mySlot.GetComponentsInChildren<Image>();
        for (int i = 0; i < mySlot.transform.childCount; i++)
        {
            skillchild[i].sprite = icon.GetComponent<Image>().sprite;
        }
        mySkillData = icon.GetComponent<SelectedSkill>().myData;
    }

    IEnumerator Cooling()
    {
        if (mySkillData != null)
        {
            myImage.fillAmount = 0.0f;
            float speed = 1.0f / mySkillData.CoolTime;
            while (myImage.fillAmount < 1.0f)
            {
                myImage.fillAmount += speed * Time.deltaTime;
                yield return null;
            }
            act = null;
        }
    }

    public void UseSkill()
    {
        GameObject skill = Instantiate(mySkillData.mySkill);
    }
}
