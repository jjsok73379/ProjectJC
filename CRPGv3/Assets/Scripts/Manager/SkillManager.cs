using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.EventSystems;

public class SkillManager : Singleton<SkillManager>
{
    public SkillData mySkill;
    public List<SkillData> mySkills = new List<SkillData>();
    public SkillData[] BasicSKills = new SkillData[6];
    public GameObject Content;
    public GameObject SkillMenu;
    public GameObject SkillPrefab;
    public bool IsOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        SkillMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            IsOpen = !IsOpen;
            SkillMenu.SetActive(IsOpen);
        }
        if (IsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                IsOpen = false;
                SkillMenu.SetActive(false);
            }
        }
    }

    public void OpenSkill()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            SkillMenu.SetActive(true);
        }
        else
        {
            IsOpen = false;
            SkillMenu.SetActive(false);
        }
    }

    public void AddSkill()
    {
        GameObject Addedskill = Instantiate(SkillPrefab, Content.transform);
        Addedskill.GetComponent<Image>().sprite = mySkill.myImage;
        Addedskill.GetComponentInChildren<TMP_Text>().text = mySkill.MyInfo;
        Addedskill.GetComponent<SkillM>().orgData = mySkill;
        mySkills.Add(mySkill);
    }

    public void Combine(SkillData skill01, SkillData skill02)
    {

    }

    public void CreateSkill(SkillData skill01, SkillData skill02)
    {

    }
}
