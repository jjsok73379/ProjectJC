using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject icon = eventData.pointerDrag.GetComponent<SkillM>().selectedSkill;
        SkillData Data = icon.GetComponent<SelectedSkill>().myData;
        mySkillData = Data;
        myImage.sprite = mySkillData.myImage;
        myText.SetActive(false);
    }
}
