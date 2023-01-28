using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public List<ItemData> itemDB = new List<ItemData>();

    public GameObject InventoryPanel;
    bool activeInventory = false;

    public InvenSlot[] invenSlots;
    public Transform slotHolder;

    private void Start()
    {
        invenSlots = slotHolder.GetComponentsInChildren<InvenSlot>();
        InventoryPanel.SetActive(activeInventory);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            activeInventory = !activeInventory;
            InventoryPanel.SetActive(activeInventory);
        }
    }

    public void RemoveItem(int _index)
    {
        itemDB.RemoveAt(_index);
    }
}
