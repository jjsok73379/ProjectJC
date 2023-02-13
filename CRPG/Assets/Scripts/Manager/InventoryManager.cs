using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Inst = null;

    public static bool inventoryActivated = false; // 인벤토리 활성화 여부. true가 되면 카메라 움직임과 다른 입력을 막을 것이다.

    [SerializeField]
    GameObject go_Inventory; // Inventory의 이미지
    [SerializeField]
    GameObject go_SlotsParent; // Slot들의 부모인 Grid Setting
    [SerializeField]
    GameObject go_QuickSlotParent;

    public Item FirstItem;

    InvenSlot[] invenSlots;
    InvenSlot[] quickSlots;
    bool isNotPut;

    bool isFull = false;

    [SerializeField]
    ActionController theActionController;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        invenSlots = go_SlotsParent.GetComponentsInChildren<InvenSlot>();
        quickSlots = go_QuickSlotParent.GetComponentsInChildren<InvenSlot>();
        go_Inventory.SetActive(false);
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
        PutSlot(quickSlots, _item, _count);
        if (isNotPut)
        {
            PutSlot(invenSlots, _item, _count);
        }
        if (isNotPut)
        {
            isFull = true;
            StartCoroutine(theActionController.WhenIventoryIsFull());
        }
    }

    public bool GetIsFull()
    {
        return isFull;
    }

    public void SetIsFull(bool _flag)
    {
        isFull = _flag;
    }

    void PutSlot(InvenSlot[] _slots,Item _item, int _count)
        {
            if (Item.ItemType.Equipment != _item.itemType)
            {
                for(int i = 0; i < _slots.Length; i++)
                {
                    if (_slots[i].item != null)
                    {
                        if (_slots[i].item.itemName == _item.itemName)
                        {
                            _slots[i].SetSlotCount(_count);
                            isNotPut = false;
                            return;
                        }
                    }
                }
            }

            for(int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item == null)
                {
                    _slots[i].AddItem(_item, _count);
                    isNotPut = false;
                    return;
                }
            }

            isNotPut=true;
        }
}
