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
    Color color;

    // Start is called before the first frame update
    void Start()
    {
        skill = SkillManager.Inst;
        inven = InventoryManager.Inst;
        color = myitemImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        inven.RedrawSlotUI();
        SlotChange();
    }

    public void UpdateSlotUI()
    {
        myitemImage.sprite = itemData.itemImage;
        color.a = 1.0f;
        myitemImage.color = color;
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
            for (int i = 0; i < skill.BasicSKills.Length; i++)
            {
                if (itemData.itemType != ItemType.SkillBook || skill.mySkills.Contains(skill.mySkill)) return;
                if (myEff.effectName == skill.BasicSKills[i].name)
                {
                    skill.mySkill = skill.BasicSKills[i];
                }
            }
            skill.AddSkill();
            inven.RemoveItem(slotnum);
        }
    }

    public void SlotChange()
    {
        for (int i = 0; i < inven.invenSlots.Length; i++)
        {
            inven.invenSlots[i].slotnum = i;
            if (i < inven.itemDB.Count)
            {
                inven.invenSlots[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                inven.invenSlots[i].GetComponent<Button>().interactable = false;
            }
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
