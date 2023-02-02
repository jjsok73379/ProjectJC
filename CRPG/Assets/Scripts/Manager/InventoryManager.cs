using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<ItemData> itemDB = new List<ItemData>();
    public ItemData FirstData;

    public GameObject InventoryPanel;
    bool activeInventory = false;

    public InvenSlot[] invenSlots;
    public Transform slotHolder;

    private void Start()
    {
        invenSlots = slotHolder.GetComponentsInChildren<InvenSlot>();
        invenSlots[0].myEff = FirstData.itemEffects[1];
        itemDB.Add(FirstData);
        InventoryPanel.SetActive(activeInventory);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            InventoryPanel.SetActive(activeInventory);
        }
        if (activeInventory)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                activeInventory = false;
                InventoryPanel.SetActive(false);
            }
        }
        if (!activeInventory)
        {
            for (int i = 0; i < invenSlots.Length; i++)
            {
                if (invenSlots[i].myInfo)
                {
                    Destroy(invenSlots[i].myInfo);
                }
            }
        }
    }

    public void SlotChange()
    {
        for (int i = 0; i < invenSlots.Length; i++)
        {
            invenSlots[i].slotnum = i;
            if (i < itemDB.Count)
            {
                invenSlots[i].GetComponent<Button>().interactable = true;
                if (invenSlots[i].IsChange)
                {
                    if (invenSlots[i + 1].myEff == null)
                    {
                        return;
                    }
                    else
                    {
                        invenSlots[i + 1].myEff = invenSlots[i + 2].myEff;
                    }
                }
            }
            else
            {
                invenSlots[i].GetComponent<Button>().interactable = false;
                invenSlots[i].itemData = null;
                invenSlots[i].myEff = null;
            }
        }
    }

    public void RedrawSlotUI()
    {
        for (int i = 0; i < invenSlots.Length; i++)
        {
            invenSlots[i].RemoveSlot();
        }
        for (int i = 0; i < itemDB.Count; i++)
        {
            invenSlots[i].itemData = itemDB[i];
            invenSlots[i].UpdateSlotUI();
        }
    }

    /*public void AlreadyHave()
    {
        for(int i = 0; i < invenSlots.Length; i++)
        {
            if (itemDB.Contains(invenSlots[i].itemData))
            {
                invenSlots[i].CountImage.SetActive(true);
            }
            else
            {
                invenSlots[i].CountImage.SetActive(false);
            }
        }
    }*/

    public void DataBase()
    {
        for (int i = 0; i < itemDB.Count; i++)
        {
            if (invenSlots[i].myEff == null)
            {
                invenSlots[i].myEff = itemDB[i].itemEffects[UnityEngine.Random.Range(0, itemDB[i].itemEffects.Count)];
            }
            if (invenSlots[i + 1] == null)
            {
                return;
            }
        }
    }

    public void RemoveItem(int _index)
    {
        itemDB.RemoveAt(_index);
    }
}
