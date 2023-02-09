using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SkillManager : Singleton<SkillManager>
{
    public SkillData mySkill;
    public List<SkillData> mySkills = new List<SkillData>();
    public SkillData[] BasicSKills = new SkillData[6];
    public List<CombinedSkillData> CombinedSkills;
    public GameObject Content;
    public GameObject CombineMenuContent;
    public GameObject SkillMenu;
    public GameObject CombineWindow;
    public GameObject myCombineMenu;
    public GameObject SkillPrefab;
    public GameObject myCombinedSkill;
    public GameObject myCombinedSkillText;
    public Sprite CombineSlotsImage;
    public Sprite CombinedSlotImage;
    public List<CombineSkill> CombineSlots;
    public bool SkillOpen = false;
    public bool CombineOpen = false;
    public bool CombineMenuOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < CombineSlots.Count; i++)
        {
            CombineSlotsImage = CombineSlots[i].GetComponent<Image>().sprite;
        }
        CombinedSlotImage = myCombinedSkill.GetComponent<Image>().sprite;
        SkillMenu.SetActive(false);
        CombineWindow.SetActive(false);
        myCombineMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SkillOpen = !SkillOpen;
            SkillMenu.SetActive(SkillOpen);
        }
        if (SkillOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SkillOpen = false;
                SkillMenu.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            CombineOpen = !CombineOpen;
            CombineWindow.SetActive(CombineOpen);
            if (CombineMenuOpen)
            {
                for(int i = 0; i < CombineSlots.Count; i++)
                {
                    CombineSlots[i].myText.SetActive(true);
                    CombineSlots[i].GetComponent<Image>().sprite = CombineSlotsImage;
                }
                myCombinedSkill.GetComponent<Image>().sprite = CombinedSlotImage;
                myCombineMenu.SetActive(false);
                CombineMenuOpen = false;
            }
        }
        if (CombineOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CombineOpen = false;
                CombineWindow.SetActive(false);
            }
        }
        else
        {
            myCombinedSkillText.SetActive(true);
        }
    }

    public void OpenSkill()
    {
        SkillOpen = !SkillOpen;
        SkillMenu.SetActive(SkillOpen);
    }

    public void SelectSkill()
    {
        CombineMenuOpen = !CombineMenuOpen;
        myCombineMenu.SetActive(CombineMenuOpen);
    }

    public void CloseMenu()
    {
        if (CombineOpen)
        {
            CombineOpen = false;
            CombineWindow.SetActive(false);
        }
        else
        {
            return;
        }
    }

    public void CloneSkill(SkillData Data)
    {
        GameObject CloneSlot = Instantiate(SkillPrefab, CombineMenuContent.transform);
        mySkill = Data;
        CloneSlot.GetComponent<Image>().sprite = mySkill.myImage;
        CloneSlot.GetComponentInChildren<TMP_Text>().text = mySkill.MyInfo;
        CloneSlot.GetComponent<SkillM>().orgData = mySkill;
    }

    public void CombineSkill()
    {
        for (int i = 0; i < CombineSlots.Count; i++)
        {
            for (int j = 0; j < CombinedSkills.Count; j++)
            {
                if (CombinedSkills[j].Materials.Contains(CombineSlots[i].mySkillData))
                {
                    myCombinedSkill.GetComponent<CombinedSkill>().mySkillData = CombinedSkills[j];
                    myCombinedSkill.GetComponent<Image>().sprite = CombinedSkills[j].myImage;
                    myCombinedSkillText.SetActive(false);
                }
            }
        }
    }

    public void AddSkill()
    {
        GameObject Addedskill = Instantiate(SkillPrefab, Content.transform);
        Addedskill.GetComponent<Image>().sprite = mySkill.myImage;
        Addedskill.GetComponentInChildren<TMP_Text>().text = mySkill.MyInfo;
        Addedskill.GetComponent<SkillM>().orgData = mySkill;
        mySkills.Add(mySkill);
        CloneSkill(mySkill);
    }
}
