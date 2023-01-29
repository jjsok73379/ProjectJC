using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    public int slotnum;
    InventoryManager inven;
    public ItemData itemData;
    public Image itemImage;
    Color color;

    // Start is called before the first frame update
    void Start()
    {
        inven = InventoryManager.Inst;
        color = itemImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        RedrawSlotUI();
        SlotChange();
    }
    public void UpdateSlotUI()
    {
        itemImage.sprite = itemData.itemImage;
        color.a = 1.0f;
        itemImage.color = color;
        itemImage.gameObject.SetActive(true);
    }
    public void RemoveSlot()
    {
        itemData = null;
        itemImage.gameObject.SetActive(false);
    }

    void RedrawSlotUI()
    {
        for (int i = 0; i < inven.invenSlots.Length; i++)
        {
            inven.invenSlots[i].RemoveSlot();
        }
        for (int i = 0; i < inven.itemDB.Count; i++)
        {
            inven.invenSlots[i].itemData = inven.itemDB[i];
            inven.invenSlots[i].UpdateSlotUI();
        }
    }

    public void UseItem()
    {
        bool isUse = itemData.Use();
        if (isUse)
        {
            if (itemData.itemType == ItemType.SkillBook)
            {
                SkillMenu.Inst.SkillBook();
            }
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
}
