using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour,IDropHandler
{
    Coroutine act = null;
    public Image myImage;
    [SerializeField] GameObject mySlot;
    public SkillData mySkillData;
    public float orgCool;
    public GameObject myText = null;

    void Start()
    {
        mySlot = gameObject;
        myText.SetActive(false);
    }

    void Update()
    {
        if (mySlot.name == "SkillQ")
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                myText.SetActive(true);
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
        if (mySlot.name == "SkillW")
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                myText.SetActive(true);
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
        if (mySlot.name == "SkillE")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                myText.SetActive(true);
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
        if (mySlot.name == "SkillR")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                myText.SetActive(true);
                if (act != null) return;
                act = StartCoroutine(Cooling());
            }
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject icon = eventData.pointerDrag.GetComponent<SkillM>().selectedSkill;
        SkillData Data = icon.GetComponent<SelectedSkill>().myData;
        mySkillData = Data;
        Image[] skillchild = mySlot.GetComponentsInChildren<Image>();//mySlot의 자식들의 이미지를 가져옴
        for (int j = 0; j < skillchild.Length; j++)
        {
            if (mySkillData != null)
            {
                orgCool = mySkillData.CoolTime;
                skillchild[j].sprite = mySkillData.myImage;//자식들 이미지에 mySkillData의 이미지를 넣음
            }
        }
        /*for (int i = 0; i < skillDatas.Length; ++i)//skillDatas의 포문
        {
            if (skillDatas.Contains(Data))
            {
                mySkillData = null;
            }
            else
            {
                mySkillData = Data; //skillDatas에 SelectedSkill의 myData를 가져옴
            }
            skillDatas[i] = mySkillData;//mySkillData에 가져온 myData를 넣음
            Image[] skillchild = mySlot.GetComponentsInChildren<Image>();//mySlot의 자식들의 이미지를 가져옴
            for (int j = 0; j < skillchild.Length; j++)
            {
                if (mySkillData != null)
                {
                    orgCool = mySkillData.CoolTime;
                    skillchild[j].sprite = mySkillData.myImage;//자식들 이미지에 mySkillData의 이미지를 넣음
                }
            }
        }*/

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
                myText.GetComponent<TMP_Text>().text = (mySkillData.CoolTime -= speed * Time.deltaTime * 10.0f).ToString("0.0");
                yield return null;
            }
            act = null;
            mySkillData.CoolTime = orgCool;
            myText.SetActive(false);
        }
    }
}
