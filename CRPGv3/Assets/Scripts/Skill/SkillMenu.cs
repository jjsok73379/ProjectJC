using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillMenu : MonoBehaviour
{
    public List<SkillData> mySkills = new List<SkillData>(); //스킬데이터 리스트4
    public SkillData[] buySkills = new SkillData[3];
    public SkillData[] BasicSKills = new SkillData[3];
    public GameObject Content;
    // Start is called before the first frame update
    void Start()
    {
        BasicSkill();
        BuySkill();
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

    public void BasicSkill()
    {
        GameObject BasicLaser = Instantiate(Resources.Load("Prefabs/SkillM"), Content.transform) as GameObject;
        BasicLaser.GetComponent<Image>().sprite = mySkills[0].myImage;
        BasicLaser.GetComponentInChildren<TMP_Text>().text = mySkills[0].MyInfo;
        BasicLaser.GetComponent<SkillM>().orgData = mySkills[0];
        BasicLaser.name = $"SkillM0";
    }

    public void BuySkill()
    {
        for (int i = 0; i < buySkills.Length; ++i)
        {
            GameObject Material = Instantiate(Resources.Load("Prefabs/SkillM"), Content.transform) as GameObject;
            Material.GetComponent<Image>().sprite = buySkills[i].myImage;
            Material.GetComponentInChildren<TMP_Text>().text = buySkills[i].MyInfo;
            Material.GetComponent<SkillM>().orgData = buySkills[i];
            Material.name = $"SkillM{i}";
        }
    }

    public void SkillBook()
    {
        for (int i = 4; i < BasicSKills.Length; ++i)
        {
            GameObject BasicSkill = Instantiate(Resources.Load("Prefabs/SkillM"), Content.transform) as GameObject;
            
        }
    }

    IEnumerator Combine(SkillData skill01, SkillData skill02)
    {
        yield return null;
    }

    IEnumerator CreateSkill(SkillData skill01, SkillData skill02)
    {
        yield return StartCoroutine(Combine(skill01, skill02));
    }
}
