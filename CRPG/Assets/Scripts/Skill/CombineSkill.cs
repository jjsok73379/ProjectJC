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
    public GameObject RemoveSKill;
    public Sprite orgImage;

    // Start is called before the first frame update
    void Start()
    {
        orgImage = GetComponent<Image>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            GameObject icon = eventData.pointerDrag.GetComponent<SkillM>().selectedSkill;
            mySkillData = icon.GetComponent<SelectedSkill>().myData;
            myImage.sprite = mySkillData.myImage;
            myText.SetActive(false);
        }
    }
}
