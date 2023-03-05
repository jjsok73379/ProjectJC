using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item; // ȹ���� ������
    public int itemCount; // ȹ���� �������� ����
    public Image itemImage; // �������� �̹���
    public GameObject Equipment_Text; // �������� ������ ǥ��
    public bool isFullSlot = false;
    public bool isEquipped = false;

    [SerializeField]
    TMP_Text text_Count;
    [SerializeField]
    GameObject go_CountImage;

    [SerializeField]
    RectTransform baseRect; // Inventory �̹����� Rect ���� �޾� ��.
    [SerializeField]
    RectTransform quickSlotBaseRect;
    [SerializeField]
    NPC_Store theNPC_Store;
    [SerializeField]
    RPGPlayer theRPGPlayer;
    [SerializeField]
    ActionController theActionController;
    InputNumber theInputNumber;
    ItemEffectDatabase theItemEffectDatabase;
    int SellPrice;

    public bool isQuickSlot; // �ش� ������ ���������� ���� �Ǵ�
    [SerializeField]
    int quickSlotNumber; // ������ �ѹ�

    // Start is called before the first frame update
    void Start()
    {
        theInputNumber = InputNumber.Inst;
        theItemEffectDatabase = ItemEffectDatabase.Inst;
    }

    // Update is called once per frame
    void Update()
    {
        if (Equipment_Text != null)
        {
            Equipment_Text.SetActive(isEquipped);
        }
        if (itemCount == 99)
        {
            isFullSlot = true;
        }
        else
        {
            isFullSlot = false;
        }
        if (item != null)
        {
            SellPrice = item.SellPrice;
        }
        else
        {
            SellPrice = 0;
        }
    }

    // ������ �̹����� ���� ����
    void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }

    public void AddItem(Item _item, int _count = 1)
    {
        item = _item;
        itemCount = _count;
        itemImage.sprite = item.itemImage;

        if (item.itemType != Item.ItemType.Equipment)
        {
            if (!isFullSlot)
            {
                go_CountImage.SetActive(true);
                text_Count.text = itemCount.ToString();
            }
        }
        else
        {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
        SetColor(1);
    }

    public void SetSlotCount(int _count)
    {
        itemCount += _count;
        if (itemCount >= 99)
        {
            itemCount = 99;
        }
        text_Count.text = itemCount.ToString();

        if(_count < 0)
        {
            if (theItemEffectDatabase.GetIsFull())
            {
                theItemEffectDatabase.SetIsFull(false);
            }
        }

        if (itemCount <= 0)
        {
            ClearSlot();
        }
    }

    void ClearSlot()
    {
        item = null;
        itemCount = 0;
        itemImage.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);

        if (theItemEffectDatabase.GetIsFull())
        {
            theItemEffectDatabase.SetIsFull(false);
        }
    }

    public int GetQuickSlotNumber()
    {
        return quickSlotNumber;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if (theNPC_Store.IsStoreOpen)
                {
                    if (item.itemType != Item.ItemType.Equipment)
                    {
                        SetSlotCount(-1);
                        theItemEffectDatabase.SellItem(item);
                    }
                    else
                    {
                        if (theRPGPlayer.mySword != null)
                        {
                            if (isEquipped)
                            {
                                StartCoroutine(theActionController.WhenCannotSell());
                            }
                            else
                            {
                                theItemEffectDatabase.SellItem(item);
                                ClearSlot();
                            }
                        }
                        else
                        {
                            theItemEffectDatabase.SellItem(item);
                            ClearSlot();
                        }
                    }
                }
                else
                {
                    if (!isQuickSlot)
                    {
                        theItemEffectDatabase.UseItem(item);
                        theItemEffectDatabase.HideToolTip();

                        if (item.itemType != Item.ItemType.Equipment)
                        {
                            SetSlotCount(-1);
                        }
                    }
                    else if (!theItemEffectDatabase.GetIsCoolTime())
                    {
                        theItemEffectDatabase.UseItem(item);
                        theItemEffectDatabase.HideToolTip();

                        if (item.itemType != Item.ItemType.Equipment)
                        {
                            SetSlotCount(-1);
                        }
                    }
                }
            }
        }
    }

    void ChangeSlot()
    {
        Item _tempItem = item;
        int _tempItemCount = itemCount;

        AddItem(DragSlot.Inst.dragSlot.item, DragSlot.Inst.dragSlot.itemCount);

        if (_tempItem == DragSlot.Inst.dragSlot.item)
        {
            _tempItemCount += DragSlot.Inst.dragSlot.itemCount;
        }

        if (_tempItem != null)
        {
            DragSlot.Inst.dragSlot.AddItem(_tempItem, _tempItemCount);
        }
        else
        {
            DragSlot.Inst.dragSlot.ClearSlot();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.Inst.dragSlot = this;
            DragSlot.Inst.DragSetImage(itemImage);
            DragSlot.Inst.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(item!= null)
        {
            DragSlot.Inst.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �κ��丮�� ������ ������ ��� ������ �巡�׸� ���´ٸ�
        if (!((DragSlot.Inst.transform.localPosition.x > baseRect.rect.xMin
            && DragSlot.Inst.transform.localPosition.x < baseRect.rect.xMax
            && DragSlot.Inst.transform.localPosition.y > baseRect.rect.yMin
            && DragSlot.Inst.transform.localPosition.y < baseRect.rect.yMax)
            ||
            (DragSlot.Inst.transform.localPosition.x + baseRect.transform.localPosition.x > quickSlotBaseRect.rect.xMin + quickSlotBaseRect.transform.localPosition.x
            && DragSlot.Inst.transform.localPosition.x + baseRect.transform.localPosition.x < quickSlotBaseRect.rect.xMax + quickSlotBaseRect.transform.localPosition.x
            && DragSlot.Inst.transform.localPosition.y + baseRect.transform.localPosition.y > quickSlotBaseRect.rect.yMin + quickSlotBaseRect.transform.localPosition.y
            && DragSlot.Inst.transform.localPosition.y + baseRect.transform.localPosition.y < quickSlotBaseRect.rect.yMax + quickSlotBaseRect.transform.localPosition.y)))
        {
            if (DragSlot.Inst.dragSlot != null)
                theInputNumber.Call();
        }
        // �κ��丮 Ȥ�� ������ �������� �巡�װ� �����ٸ�
        else
        {
            DragSlot.Inst.SetColor(0);
            DragSlot.Inst.dragSlot = null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.Inst.dragSlot != null)
        {
            ChangeSlot();

            if (isQuickSlot)
            {
                theItemEffectDatabase.IsActivatedquickSlot(quickSlotNumber);
            }
            else
            {
                if (DragSlot.Inst.dragSlot.isQuickSlot)
                {
                    theItemEffectDatabase.IsActivatedquickSlot(DragSlot.Inst.dragSlot.quickSlotNumber);
                }
            }
        }
        else return;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            if (!isQuickSlot)
            {
                theItemEffectDatabase.ShowToolTip(item, transform.position, SellPrice);
            }
            else
            {
                Vector3 quickslotPos = new Vector3(transform.position.x, transform.position.y + 500.0f, transform.position.z);
                theItemEffectDatabase.ShowToolTip(item, quickslotPos, SellPrice);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theItemEffectDatabase.HideToolTip();
    }
}
