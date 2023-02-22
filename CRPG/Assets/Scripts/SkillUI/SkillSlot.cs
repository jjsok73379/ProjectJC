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
    public Image myImage;
    [SerializeField]
    Image[] skillImages;
    [SerializeField]
    SkillManager theSkillManager;
    [SerializeField]
    RPGPlayer theRPGPlayer;
    Sprite[] orgImages = new Sprite[2];
    public SkillData mySkillData;
    public float orgCool;
    public float mySkillDamage;
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
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            mySkillData = null;
            myImage = null;
            orgCool = 0;
            for (int i = 0; i < skillImages.Length; i++)
            {
                skillImages[i].sprite = orgImages[i];
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Level = eventData.pointerDrag.GetComponent<SkillM>().myStat.Level;
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

    public IEnumerator Cooling()
    {
        for (int i = 0; i < theSkillManager.SkillSlots.Length; i++)
        {
            if (theSkillManager.SkillSlots[i].mySkillData != null)
            {
                theSkillManager.SkillSlots[i].myImage.fillAmount = 0.0f;
                float speed = 1.0f / theSkillManager.SkillSlots[i].mySkillData.CoolTime;
                while (theSkillManager.SkillSlots[i].myImage.fillAmount < 1.0f)
                {
                    theSkillManager.SkillSlots[i].myImage.fillAmount += speed * Time.deltaTime;
                    yield return null;
                }
                theRPGPlayer.act = null;
                theSkillManager.SkillSlots[i].mySkillData.CoolTime = theSkillManager.SkillSlots[i].orgCool;
            }
        }
    }
}
