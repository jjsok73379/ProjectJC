using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombineSkill : MonoBehaviour,IDropHandler
{
    public SkillData mySkillData;
    public GameObject mySlot;
    public GameObject myText;
    public Image myImage;
    public Sprite orgImage;

    // Start is called before the first frame update
    void Start()
    {
        orgImage = myImage.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject icon = eventData.pointerDrag.GetComponent<SkillM>().selectedSkill;
        SkillData Data = icon.GetComponent<SelectedSkill>().myData;
        if (!SkillManager.Inst.BasicSkills.Contains(Data)) return;
        else if(SkillManager.Inst.BasicSkills.Contains(Data))
        {
            mySkillData = Data;
        }
        if (mySkillData != null)
        {
            myImage.sprite = mySkillData.myImage;
            myText.SetActive(false);
        }
    }
}
