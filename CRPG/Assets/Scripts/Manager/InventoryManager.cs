using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{

    public bool inventoryActivated = false; // 인벤토리 활성화 여부. true가 되면 카메라 움직임과 다른 입력을 막을 것이다.

    [SerializeField]
    GameObject go_Inventory; // Inventory의 이미지
    [SerializeField]
    GameObject go_SlotsParent; // Slot들의 부모인 Grid Setting
    [SerializeField]
    GameObject go_QuickSlotParent;

    [SerializeField]
    Item FirstItem;
    [SerializeField] 
    Item SecondItem; // 추후 삭제
    [SerializeField]
    Item ThirdItem; // 추후 삭제
    public int itemMaxCount = 99; // 아이템의 최대 개수

    public InvenSlot[] allSlots;
    InvenSlot[] invenSlots;
    InvenSlot[] quickSlots;
    bool isNotPut;
    int tempcount;
    int sumcount;

    bool isFull = false;

    [SerializeField]
    ActionController theActionController;
    [SerializeField]
    ItemEffectDatabase theItemEffectDatabase;

    // Start is called before the first frame update
    void Start()
    {
        invenSlots = go_SlotsParent.GetComponentsInChildren<InvenSlot>();
        quickSlots = go_QuickSlotParent.GetComponentsInChildren<InvenSlot>();
        go_Inventory.SetActive(false);
        invenSlots[0].AddItem(FirstItem);
        invenSlots[1].AddItem(SecondItem);
        invenSlots[2].AddItem(ThirdItem);
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
        for(int i = 0; i < allSlots.Length; i++)
        {
            if (!allSlots[i].isQuickSlot)
            {
                if (!inventoryActivated)
                {
                    theItemEffectDatabase.HideToolTip();
                }
            }
        }
    }

    void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;
            go_Inventory.SetActive(inventoryActivated);
        }
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if (_item.itemType != Item.ItemType.Equipment)
        {
            PutSlot(quickSlots, _item, _count);
        }
        else
        {
            PutSlot(invenSlots, _item, _count);
        }
        if (isNotPut)
        {
            PutSlot(invenSlots, _item, _count);
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

    void PutSlot(InvenSlot[] _slots, Item _item, int _count)
    {
        if (Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].item != null)
                {
                    if (_slots[i].item.itemName == _item.itemName)
                    {
                        if (!_slots[i].isFullSlot)
                        {
                            sumcount = _slots[i].itemCount + _count;
                            tempcount = sumcount - itemMaxCount;
                            if (sumcount >= itemMaxCount)
                            {
                                _count = itemMaxCount;
                                _slots[i].SetSlotCount(_count);
                                isFull = true;
                                if (tempcount > 0)
                                {
                                    if (_slots[i + 1] == null) return;
                                    _slots[i + 1].AddItem(_item, tempcount);
                                }
                            }
                            isNotPut = false;
                            return;
                        }
                    }
                }
            }
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i].item == null)
            {
                _slots[i].AddItem(_item, _count);
                isNotPut = false;
                return;
            }
        }

        isNotPut = true;
    }
}
