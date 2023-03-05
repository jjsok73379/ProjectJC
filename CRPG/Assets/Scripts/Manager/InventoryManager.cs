using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    [SerializeField]
    Item[] items;

    public InvenSlot[] GetSlots() { return allSlots; }

    [SerializeField]
    GameObject go_SlotsParent; // Slot들의 부모인 Grid Setting
    [SerializeField]
    GameObject go_QuickSlotParent;

    [SerializeField]
    Item FirstItem;
    [SerializeField] 
    Item SecondItem;
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
        invenSlots[0].AddItem(FirstItem);
        invenSlots[1].AddItem(SecondItem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadToSlot(int _arrayNum, string _itemName, int _itemNum)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].itemName == _itemName)
            {
                allSlots[_arrayNum].AddItem(items[i], _itemNum);
            }
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
