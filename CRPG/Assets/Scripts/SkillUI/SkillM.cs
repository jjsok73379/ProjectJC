using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public struct SkillStat
{
    public SkillData orgData;
    public int Level;
    public SkillData Material
    {
        get => orgData.Materials[2];
    }
    public int MaterialCount
    {
        get => orgData.GetMaterialCount(Level);
    }
    public int UpgradeCount
    {
        get => orgData.GetMaterialCount(Level + 1);
    }
    public int TotalCount
    {
        get
        {
            int total = 0;
            for (int i = 1; i <= Level; i++)
            {
                total += orgData.GetMaterialCount(i);
            }
            return total;
        }
    }
    public bool IsUpgradable
    {
        get => Level < orgData.GetMaxLevel();
    }
    public float SkillDamage
    {
        get => orgData.SkillDamage(Level);
    }
    public float AttackRange
    {
        get => orgData.GetAttackRange();
    }
}
public class SkillM : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public SkillStat myStat;
    public int MaterialCount
    {
        get => myStat.MaterialCount;
    }
    public int UpgradeCount
    {
        get => myStat.UpgradeCount;
    }
    public int TotalCount
    {
        get => myStat.TotalCount;
    }
    public bool IsUpgradable
    {
        get => myStat.IsUpgradable;
    }

    public void OnUpgrade()
    {
        myStat.Level++;
    }

    Vector2 dragOffset = Vector2.zero;
    public GameObject selectedSkill;
    public GameObject myReinfoce;
    [SerializeField]
    TMP_Text ReinforceCount;
    [SerializeField]
    GameObject CombinedSkillInfo;
    [SerializeField]
    GameObject MaterialSkillInfo;
    ActionController theActionController;
    [SerializeField]
    MaterialSlot[] materialSlots = new MaterialSlot[2];

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 pos = Input.mousePosition;
        selectedSkill = Instantiate(Resources.Load("Prefabs/SelectedSkill"), pos, Quaternion.identity, transform.parent.parent.parent.parent) as GameObject;
        selectedSkill.GetComponent<SelectedSkill>().myData = myStat.orgData;
        dragOffset = (Vector2)selectedSkill.transform.localPosition - eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        selectedSkill.transform.localPosition = eventData.position + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Destroy(selectedSkill);
    }

    // Start is called before the first frame update
    void Start()
    {
        theActionController = ActionController.Inst;
        materialSlots = SkillManager.Inst.theMaterialSlots;
        SkillManager.Inst.ReinforceOpen = false;
        SkillManager.Inst.ReinforceWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ShowReinforceButton();
        if (!OpenManager.Inst.SkillActivated)
        {
            SkillManager.Inst.ReinforceWindow.SetActive(false);
        }
        if (SkillManager.Inst.CombinedSkills.Contains(myStat.orgData))
        {
            CombinedSkillInfo.SetActive(true);
            MaterialSkillInfo.SetActive(false);
            ReinforceCount.text = "+ " + myStat.Level.ToString();
        }
        else
        {
            CombinedSkillInfo.SetActive(false);
            MaterialSkillInfo.SetActive(true);
        }
    }

    public void ShowReinforceButton()
    {
        for (int i = 0; i < SkillManager.Inst.CombinedSkills.Count; i++)
        {
            if (SkillManager.Inst.CombinedSkills.Contains(myStat.orgData))
            {
                myReinfoce.SetActive(true);
            }
            else
            {
                myReinfoce.SetActive(false);
            }
        }
    }
    public void OpenReinFoce()
    {
        SkillManager.Inst.ReinforceOpen = !SkillManager.Inst.ReinforceOpen;
        SkillManager.Inst.ReinforceWindow.SetActive(SkillManager.Inst.ReinforceOpen);
        SkillManager.Inst.ReinforceImage.sprite = myStat.orgData.myImage;
        for (int j = 0; j < materialSlots.Length; j++)
        {
            materialSlots[j].myCount.text = myStat.MaterialCount.ToString();
        }
        UpgradeSKill(SkillManager.Inst);
    }

    public void UpgradeSKill(SkillManager skill)
    {
        int skillbook1 = 0;
        int skillbook2 = 0;
        skill.UpgradeButton.onClick.RemoveAllListeners();
        if (myStat.IsUpgradable)
        {
            skill.UpgradeButton.interactable = true;
            skill.UpgradeButton.onClick.AddListener(
                () =>
                {
                    if (myStat.IsUpgradable)
                    {
                        for (int i = 0; i < InventoryManager.Inst.allSlots.Length; i++)
                        {
                            if (myStat.MaterialCount == skillbook1 && myStat.MaterialCount == skillbook2)
                            {
                                Debug.Log("°­È­¼º°ø");
                                SkillManager.Inst.ReinforceWindow.SetActive(false);
                                SkillManager.Inst.ReinforceOpen = false;
                                OnUpgrade();
                                break;
                            }
                            else
                            {
                                if (InventoryManager.Inst.allSlots[i].item != null)
                                {
                                    if (InventoryManager.Inst.allSlots[i].item.itemType == Item.ItemType.SkillBook)
                                    {
                                        if (InventoryManager.Inst.allSlots[i].item.itemName == $"½ºÅ³ºÏ : {myStat.orgData.Materials[0].SkillName}")
                                        {
                                            if (InventoryManager.Inst.allSlots[i].itemCount >= myStat.MaterialCount)
                                            {
                                                InventoryManager.Inst.allSlots[i].SetSlotCount(-myStat.MaterialCount);
                                                skillbook1++;
                                            }
                                            else
                                            {
                                                Debug.Log(myStat.MaterialCount);
                                                Debug.Log(InventoryManager.Inst.allSlots[i].itemCount);
                                                StartCoroutine(theActionController.WhenLessCount());
                                            }
                                        }
                                        else if (InventoryManager.Inst.allSlots[i].item.itemName == $"½ºÅ³ºÏ : {myStat.orgData.Materials[1].SkillName}")
                                        {
                                            if (InventoryManager.Inst.allSlots[i].itemCount >= myStat.MaterialCount)
                                            {
                                                InventoryManager.Inst.allSlots[i].SetSlotCount(-myStat.MaterialCount);
                                                skillbook2++;
                                            }
                                            else
                                            {
                                                Debug.Log(myStat.MaterialCount);
                                                Debug.Log(InventoryManager.Inst.allSlots[i].itemCount);
                                                StartCoroutine(theActionController.WhenLessCount());
                                            }
                                        }
                                        else
                                        {
                                            StartCoroutine(theActionController.WhenDontHave());
                                        }
                                    }
                                    else
                                    {
                                        StartCoroutine(theActionController.WhenDontHave());
                                    }
                                }
                                else
                                {
                                    StartCoroutine(theActionController.WhenDontHave());
                                }
                            }
                        }
                    }
                    else
                    {
                        skill.UpgradeButton.interactable = false;
                    }
                }
                );
        }
        else
        {
            skill.UpgradeButton.interactable = false;
        }
    }
}
