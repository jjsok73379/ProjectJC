using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour,IDropHandler
{
    Coroutine act = null;
    public Image myImage;
    [SerializeField] 
    GameObject mySlot;
    [SerializeField]
    Image[] skillImages;
    public SkillData mySkillData;
    public float orgCool;

    void Start()
    {
        mySlot = gameObject;
    }

    void Update()
    {
        if (mySlot.name == "SkillQ")
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
        if (mySlot.name == "SkillW")
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
        if (mySlot.name == "SkillE")
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
        if (mySlot.name == "SkillR")
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject icon = eventData.pointerDrag.GetComponent<SkillM>().selectedSkill;
        SkillData Data = icon.GetComponent<SelectedSkill>().myData;
        if (SkillManager.Inst.SkillSlotDatas.Contains(Data)) return;
        mySkillData = Data;
        for (int i = 0; i < skillImages.Length; i++)
        {
            if (mySkillData != null)
            {
                for (int j = 0; j < SkillManager.Inst.SkillSlots.Length; j++)
                {
                    orgCool = mySkillData.CoolTime;
                    skillImages[i].sprite = mySkillData.myImage;
                    //자식들 이미지에 mySkillData의 이미지를 넣음
                }
            }
        }
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
            mySkillData.CoolTime = orgCool;
        }
    }
}
