using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Data;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Inst;

    [SerializeField]
    SkillData[] skillDatas;

    public List<SkillData> GetSkills() { return mySkills; }

    public GameObject AddedskillPanel;

    public SkillData mySkill;
    public List<SkillData> mySkills = new List<SkillData>();
    public SkillData[] BasicSkills = new SkillData[3];
    public SkillData[] Materials = new SkillData[3];
    public List<SkillData> CombinedSkills;

    public GameObject[] Contents;

    public GameObject myCombinedSkill;
    public GameObject myCombineMenu;
    public GameObject myCombinedSkillText;

    public MaterialSlot[] theMaterialSlots;
    public GameObject ReinforceWindow;
    public Image ReinforceImage;
    public Button UpgradeButton;

    public GameObject SkillPrefab;

    public CombineSkill CombineSkillSlot;
    public CombineMaterial CombineMaterialSlot;
    public bool CombineMenuOpen = false;
    public bool ReinforceOpen = false;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (myCombineMenu != null)
        {
            myCombineMenu.SetActive(false);
        }
        if (DataManager.Inst.skills.Count > 0 && mySkills.Count == 0)
        {
            for(int i = 0; i < DataManager.Inst.skills.Count;)
            {
                mySkill = DataManager.Inst.skills[i];
                AddSkill();
                i++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CombineMenuOpen)
        {
            CombineSkillSlot.myText.SetActive(true);
            CombineMaterialSlot.myText.SetActive(true);
            myCombineMenu.SetActive(false);
            CombineMenuOpen = false;
        }
    }

   public void LoadToSkill(SkillData skilldata)
    {
        for (int i = 0; i < mySkills.Count; i++)
        {
            if (skillDatas[i] == skilldata)
            {
                mySkills.Add(skillDatas[i]);
            }
        }
    }

    public void SelectSkill()
    {
        CombineMenuOpen = !CombineMenuOpen;
        myCombineMenu.SetActive(CombineMenuOpen);
    }

    public void CloseReinforce()
    {
        ReinforceOpen = false;
        ReinforceWindow.SetActive(false);
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
            AddedskillPanel = Instantiate(SkillPrefab, Contents[i].transform);
            SkillM Addedskill = AddedskillPanel.GetComponentInChildren<SkillM>();
            Addedskill.GetComponent<Image>().sprite = mySkill.myImage;
            Addedskill.GetComponentInChildren<TMP_Text>().text = mySkill.MyInfo;
            Addedskill.GetComponent<SkillM>().myStat.orgData = mySkill;
        }
        mySkills.Add(mySkill);
        if(!DataManager.Inst.skills.Contains(mySkill))
        {
            DataManager.Inst.skills.Add(mySkill);
        }
    }
}
