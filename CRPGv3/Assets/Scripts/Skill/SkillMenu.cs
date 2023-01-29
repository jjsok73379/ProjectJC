using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SkillMenu : Singleton<SkillMenu>
{
    public List<SkillData> mySkills = new List<SkillData>(); //스킬데이터 리스트4
    public SkillData[] BasicSKills = new SkillData[6];
    public GameObject Content;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CloseMenu()
    {
        SkillManager.Inst.IsOpen = false;
        SkillManager.Inst.SkillMenu.SetActive(false);
    }

    public void ClickBuy()
    {

    }

    public void SkillBook()
    {
        for (int i = 0; i < BasicSKills.Length; ++i)
        {
            GameObject BasicSkill = Instantiate(Resources.Load("Prefabs/SkillM"), Content.transform) as GameObject;
            BasicSkill.GetComponent<Image>().sprite = BasicSKills[i].myImage;
            BasicSkill.GetComponentInChildren<TMP_Text>().text = BasicSKills[i].MyInfo;
            BasicSkill.GetComponent<SkillM>().orgData = BasicSKills[i];
            mySkills.Add(BasicSKills[i]);
        }
    }

    public void MySkill()
    {
        for (int i = 0; i < mySkills.Count; ++i)
        {

        }
    }

    public void Combine(SkillData skill01, SkillData skill02)
    {
        
    }

    public void CreateSkill(SkillData skill01, SkillData skill02)
    {
        
    }
}
