using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SkillSlot : MonoBehaviour, IPointerClickHandler, IDropHandler
{
    public int mySlotNum;
    public Image myImage;
    [SerializeField]
    Image[] skillImages;
    Sprite[] orgImages = new Sprite[2];
    public SkillData mySkillData;
    public float orgCool;
    public float mySkillDamage;
    public float mySkillRange;
    public bool IsCooling = false;
    int Level;

    private void Start()
    {
        for (int i = 0; i < skillImages.Length; i++)
        {
            orgImages[i] = skillImages[i].sprite;
        }
    }

    private void Update()
    {
        if (mySkillData != null)
        {
            mySkillDamage = mySkillData.SkillDamage(Level);
            mySkillRange = mySkillData.GetAttackRange();
            for (int i = 0; i < skillImages.Length; i++)
            {
                orgCool = mySkillData.CoolTime;
                skillImages[i].sprite = mySkillData.myImage;
                //자식들 이미지에 mySkillData의 이미지를 넣음
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DataManager.Inst.SkillSlotDatas[mySlotNum - 1] = null;
            mySkillData = null;
            myImage.fillAmount = 1;
            orgCool = 0;
            for (int i = 0; i < skillImages.Length; i++)
            {
                skillImages[i].sprite = orgImages[i];
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Transform DropPos = eventData.pointerDrag.transform;
        if (!DropPos.GetComponent<SkillM>()) return;
        Level = eventData.pointerDrag.GetComponent<SkillM>().myStat.Level;
        GameObject icon = eventData.pointerDrag.GetComponent<SkillM>().selectedSkill;
        SkillData Data = icon.GetComponent<SelectedSkill>().myData;
        if (DataManager.Inst.SkillSlotDatas.Contains(Data)) return;
        else
        {
            if (mySkillData != null)
            {
                DataManager.Inst.SkillSlotDatas[mySlotNum - 1] = null;
            }
            DataManager.Inst.SkillSlotDatas[mySlotNum - 1] = Data;
        }
        mySkillData = Data;
        for (int i = 0; i < skillImages.Length; i++)
        {
            if (mySkillData != null)
            {
                orgCool = mySkillData.CoolTime;
                skillImages[i].sprite = mySkillData.myImage;
                //자식들 이미지에 mySkillData의 이미지를 넣음
            }
        }
    }

    public IEnumerator Cooling()
    {
        if (mySkillData != null)
        {
            IsCooling = true;
            mySkillData.CoolTime = orgCool;
            myImage.fillAmount = 0.0f;
            float speed = 1.0f / mySkillData.CoolTime;
            while (myImage.fillAmount < 1.0f)
            {
                myImage.fillAmount += speed * Time.deltaTime;
                yield return null;
            }
            IsCooling = false;
        }
    }
}
