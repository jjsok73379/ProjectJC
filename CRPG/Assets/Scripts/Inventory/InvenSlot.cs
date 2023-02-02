using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public int slotnum;
    InventoryManager inven;
    SkillManager skill;
    public ItemData itemData;
    public Image myitemImage;
    public GameObject myInfo;
    public GameObject CountImage;
    public ItemEffect myEff;
    public bool IsChange = false;

    // Start is called before the first frame update
    void Start()
    {
        skill = SkillManager.Inst;
        inven = InventoryManager.Inst;
        myitemImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        inven.RedrawSlotUI();
        inven.SlotChange();
    }

    public void UpdateSlotUI()
    {
        myitemImage.sprite = itemData.itemImage;
        myitemImage.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        itemData = null;
        myitemImage.gameObject.SetActive(false);
    }

    public void UseItem()
    {
        bool isUse = itemData.Use();
        if (isUse)
        {
            IsChange = true;
            for (int i = 0; i < skill.BasicSKills.Length; i++)
            {
                if (itemData.itemType != ItemType.SkillBook ) return;
                if (myEff.effectName == skill.BasicSKills[i].name)
                {
                    skill.mySkill = skill.BasicSKills[i];
                    if (!skill.mySkills.Contains(skill.mySkill))
                    {
                        skill.mySkill = skill.BasicSKills[i];
                    }
                    else
                    {
                        return;
                    }
                }
            }
            skill.AddSkill();
            for(int i = 0; i < inven.invenSlots.Length; i++)
            {
                if (inven.invenSlots[i].myEff != null)
                {
                    if (inven.invenSlots[i + 1] == null)
                    {
                        return;
                    }
                    inven.invenSlots[i].myEff = inven.invenSlots[i + 1].myEff;
                }
            }
            inven.RemoveItem(slotnum);
            skill.mySkill = null;
            IsChange = false;
        }
    }

    public void WriteInfo()
    {
        Vector3 pos = transform.position;
        pos.y = pos.y + 200.0f;
        myInfo = Instantiate(Resources.Load("Prefabs/ItemInfo"), pos, Quaternion.identity, inven.InventoryPanel.transform) as GameObject;
        myInfo.GetComponentInChildren<TMP_Text>().text = itemData.itemName + " : " + myEff.effectName + "\n" + itemData.ItemEff;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemData != null)
        {
            WriteInfo();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(myInfo);
    }
}
