using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지
    public GameObject Equipment_Text; // 장착중인 아이템 표시
    public bool isFullSlot = false;
    public bool isEquipped = false;

    [SerializeField]
    TMP_Text text_Count;
    [SerializeField]
    GameObject go_CountImage;

    [SerializeField]
    RectTransform baseRect; // Inventory 이미지의 Rect 정보 받아 옴.
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

    public bool isQuickSlot; // 해당 슬롯이 퀵슬롯인지 여부 판단
    [SerializeField]
    int quickSlotNumber; // 퀵슬롯 넘버

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

    // 아이템 이미지의 투명도 조절
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
            go_CountImage.SetActive(true);
            text_Count.text = itemCount.ToString();
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
                if (theNPC_Store != null)
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
                }
                if (!isQuickSlot || !theItemEffectDatabase.GetIsCoolTime())
                {
                    theItemEffectDatabase.UseItem(item);
                    theItemEffectDatabase.HideToolTip();

                    if (item.itemType != Item.ItemType.Equipment)
                    {
                        if (!theItemEffectDatabase.isAlreadyHave)
                        {
                            SetSlotCount(-1);
                        }
                        else
                        {
                            theItemEffectDatabase.isAlreadyHave = false;
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
        int temp = 0;


        if (_tempItem != null)
        {
            if (_tempItem == DragSlot.Inst.dragSlot.item)
            {
                if (_tempItemCount + DragSlot.Inst.dragSlot.itemCount >= 99)
                {
                    temp = _tempItemCount + DragSlot.Inst.dragSlot.itemCount - 99;
                    _tempItemCount = 99;
                    DragSlot.Inst.dragSlot.AddItem(DragSlot.Inst.dragSlot.item, temp);
                }
                else
                {
                    _tempItemCount += DragSlot.Inst.dragSlot.itemCount;
                    DragSlot.Inst.dragSlot.ClearSlot();
                }
                AddItem(_tempItem, _tempItemCount);
            }
            else
            {
                AddItem(DragSlot.Inst.dragSlot.item, DragSlot.Inst.dragSlot.itemCount);
                DragSlot.Inst.dragSlot.AddItem(_tempItem, _tempItemCount);
            }

        }
        else
        {
            AddItem(DragSlot.Inst.dragSlot.item, DragSlot.Inst.dragSlot.itemCount);
            DragSlot.Inst.dragSlot.ClearSlot();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            SoundManager.Inst.ButtonSound.Play();
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
        if(DragSlot.Inst.dragSlot != null)
        {
            // 인벤토리와 퀵슬롯 영역을 벗어난 곳에서 드래그를 끝냈다면
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
            // 인벤토리 혹은 퀵슬롯 영역에서 드래그가 끝났다면
            else
            {
                DragSlot.Inst.SetColor(0);
                DragSlot.Inst.dragSlot = null;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Transform DropPos = eventData.pointerDrag.transform;
        if (!DropPos.GetComponent<InvenSlot>()) return;
        SoundManager.Inst.ButtonSound.Play();
        if (DragSlot.Inst.dragSlot != null)
        {
            ChangeSlot();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            if (!isQuickSlot)
            {
                theItemEffectDatabase.ShowToolTip(item, transform.position - new Vector3(250.0f, -50.0f, 0), SellPrice);
            }
            else
            {
                Vector3 quickslotPos = new Vector3(transform.position.x - 250.0f, transform.position.y + 350.0f, transform.position.z);
                theItemEffectDatabase.ShowToolTip(item, quickslotPos, SellPrice);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theItemEffectDatabase.HideToolTip();
    }
}
