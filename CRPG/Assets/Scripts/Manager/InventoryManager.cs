using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Inst = null;
    private void Awake()
    {
        Inst = this;
    }


    public static bool inventoryActivated = false; // �κ��丮 Ȱ��ȭ ����. true�� �Ǹ� ī�޶� �����Ӱ� �ٸ� �Է��� ���� ���̴�.

    [SerializeField]
    GameObject go_Inventory; // Inventory�� �̹���
    [SerializeField]
    GameObject go_SlotsParent; // Slot���� �θ��� Grid Setting
    public Item FirstItem;

    InvenSlot[] invenSlots;


    // Start is called before the first frame update
    void Start()
    {
        invenSlots = go_SlotsParent.GetComponentsInChildren<InvenSlot>();
        go_Inventory.SetActive(false);
        FirstItem.itemName = "BasicLaser";
        invenSlots[0].AddItem(FirstItem);
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
    }

    void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    void OpenInventory()
    {
        go_Inventory.SetActive(true);
    }

    void CloseInventory()
    {
        go_Inventory.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < invenSlots.Length; i++)
            {
                if (invenSlots[i].item != null)
                {
                    if (invenSlots[i].item.itemName == _item.itemName)
                    {
                        invenSlots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < invenSlots.Length; i++)
        {
            if (invenSlots[i].item == null)
            {
                invenSlots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}
