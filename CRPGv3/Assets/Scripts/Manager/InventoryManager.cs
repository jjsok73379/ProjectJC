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

    public void DataBase()
    {
        for (int i = 0; i < itemDB.Count; i++)
        {
            invenSlots[i].myEff = itemDB[i].itemEffects[Random.Range(0, itemDB[i].itemEffects.Count - 1)];
        }
    }

    public void RemoveItem(int _index)
    {
        itemDB.RemoveAt(_index);
    }
}
