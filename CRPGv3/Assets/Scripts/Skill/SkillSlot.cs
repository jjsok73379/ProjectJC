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
        Image[] skillchild = mySlot.GetComponentsInChildren<Image>();//mySlot�� �ڽĵ��� �̹����� ������
        for (int j = 0; j < skillchild.Length; j++)
        {
            if (mySkillData != null)
            {
                orgCool = mySkillData.CoolTime;
                skillchild[j].sprite = mySkillData.myImage;//�ڽĵ� �̹����� mySkillData�� �̹����� ����
            }
        }
        /*for (int i = 0; i < skillDatas.Length; ++i)//skillDatas�� ����
        {
            if (skillDatas.Contains(Data))
            {
                mySkillData = null;
            }
            else
            {
                mySkillData = Data; //skillDatas�� SelectedSkill�� myData�� ������
            }
            skillDatas[i] = mySkillData;//mySkillData�� ������ myData�� ����
            Image[] skillchild = mySlot.GetComponentsInChildren<Image>();//mySlot�� �ڽĵ��� �̹����� ������
            for (int j = 0; j < skillchild.Length; j++)
            {
                if (mySkillData != null)
                {
                    orgCool = mySkillData.CoolTime;
                    skillchild[j].sprite = mySkillData.myImage;//�ڽĵ� �̹����� mySkillData�� �̹����� ����
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
