using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class SkillManager : Singleton<SkillManager>
{
    public List<SkillData> SkillSlotDatas;
    public SkillSlot[] SkillSlots;

    public SkillData mySkill;
    public List<SkillData> mySkills = new List<SkillData>();
    public SkillData[] BasicSkills = new SkillData[3];
    public SkillData[] Materials = new SkillData[3];
    public List<CombinedSkillData> CombinedSkills;

    public GameObject[] Contents;

    public GameObject SkillMenu;

    public GameObject CombineWindow;
    public GameObject myCombineMenu;
    public GameObject myCombinedSkill;
    public GameObject myCombinedSkillText;

    public GameObject ReinforceWindow;
    public GameObject ReinfoceSkill;

    public GameObject SkillPrefab;

    public CombineSkill CombineSkillSlot;
    public CombineMaterial CombineMaterialSlot;
    public bool SkillOpen = false;
    public bool CombineOpen = false;
    public bool CombineMenuOpen = false;
    public bool ReinforceOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        ReinforceOpen = false;
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
            ReinforceWindow.SetActive(false);
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
                CombineSkillSlot.myText.SetActive(true);
                CombineMaterialSlot.myText.SetActive(true);
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
        SkillSlotDatas[0] = SkillSlots[0].mySkillData;
        SkillSlotDatas[1] = SkillSlots[1].mySkillData;
        SkillSlotDatas[2] = SkillSlots[2].mySkillData;
        SkillSlotDatas[3] = SkillSlots[3].mySkillData;
    }

    public void OpenSkill()
    {
        SkillOpen = !SkillOpen;
        SkillMenu.SetActive(SkillOpen);
    }

    public void OpenReinFoce()
    {
        ReinforceOpen = !ReinforceOpen;
        ReinforceWindow.SetActive(ReinforceOpen);
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

    public void CombineSkill()
    {
        if (CombineSkillSlot.mySkillData == null || CombineMaterialSlot.mySkillData == null) return;
        for (int i = 0; i < CombinedSkills.Count; i++)
        {
            if (CombinedSkills[i].Materials[0] == CombineSkillSlot.mySkillData && CombinedSkills[i].Materials[1] == CombineMaterialSlot.mySkillData)
            {
                myCombinedSkill.GetComponent<CombinedSkill>().mySkillData = CombinedSkills[i];
                myCombinedSkill.GetComponent<Image>().sprite = CombinedSkills[i].myImage;
                myCombinedSkillText.SetActive(false);
            }
        }
        CombineSkillSlot.myImage.sprite = CombineSkillSlot.orgImage;
        CombineMaterialSlot.myImage.sprite = CombineMaterialSlot.orgImage;
        CombineSkillSlot.myText.SetActive(true);
        CombineMaterialSlot.myText.SetActive(true);
    }

    public void AddSkill()
    {
        for(int i = 0; i < Contents.Length; i++)
        {
            GameObject AddedskillPanel = Instantiate(SkillPrefab, Contents[i].transform);
            SkillM Addedskill = AddedskillPanel.GetComponentInChildren<SkillM>();
            Addedskill.GetComponent<Image>().sprite = mySkill.myImage;
            Addedskill.GetComponentInChildren<TMP_Text>().text = mySkill.MyInfo;
            Addedskill.GetComponent<SkillM>().orgData = mySkill;
        }
        mySkills.Add(mySkill);
    }
}
