using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item item; // 획득한 아이템
    public int itemCount; // 획득한 아이템의 개수
    public Image itemImage; // 아이템의 이미지

    [SerializeField]
    TMP_Text text_Count;
    [SerializeField]
    GameObject go_CountImage;
    [SerializeField]
    Image _highlightImage;

    GameObject _highlightGo;

    float _highlightAlpha = 0.5f;
    float _highlightFadeDuration = 0.2f;
    float _currentHLAlpha = 0f;

    Rect baseRect; // Inventory 이미지의 Rect 정보 받아 옴.
    WeaponManager theWeaponManager;
    InputNumber theInputNumber;

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
        text_Count.text = itemCount.ToString();

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
    }

    // Start is called before the first frame update
    void Start()
    {
        theWeaponManager = WeaponManager.Inst;
        theInputNumber = InputNumber.Inst;
        baseRect = transform.parent.parent.GetComponent<RectTransform>().rect;
        _highlightGo = _highlightImage.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null)
            {
                if(item.itemType == Item.ItemType.Equipment)
                {
                    //장착
                    StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(item.itemName));
                }
                else
                {
                    //소비
                    Debug.Log(item.itemName + " 을 사용했습니다. ");
                    SetSlotCount(-1);
                }
            }
        }
    }

    public void Highlight(bool show)
    {
        if (show)
        {
            StartCoroutine(nameof(HighlightFadeInRoutine));
        }
        else
        { 
            StartCoroutine(nameof(HighlightFadeOutRoutine));
        }
    }

    IEnumerator HighlightFadeInRoutine()
    {
        StopCoroutine(nameof(HighlightFadeOutRoutine));
        _highlightGo.SetActive(true);

        float unit = _highlightAlpha / _highlightFadeDuration;

        for (; _currentHLAlpha <= _highlightAlpha; _currentHLAlpha += unit * Time.deltaTime)
        {
            _highlightImage.color = new Color(
                    _highlightImage.color.r,
                    _highlightImage.color.g,
                    _highlightImage.color.b,
                    _currentHLAlpha
                );

            yield return null;
        }
    }

    IEnumerator HighlightFadeOutRoutine()
    {
        StopCoroutine(nameof(HighlightFadeInRoutine));

        float unit = _highlightAlpha / _highlightFadeDuration;

        for (; _currentHLAlpha >= 0f; _currentHLAlpha -= unit * Time.deltaTime)
        {
            _highlightImage.color = new Color(
                _highlightImage.color.r,
                _highlightImage.color.g,
                _highlightImage.color.b,
                _currentHLAlpha
            );

            yield return null;
        }

        _highlightGo.SetActive(false);
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
        if(DragSlot.Inst.transform.localPosition.x<baseRect.xMin
            ||DragSlot.Inst.transform.localPosition.x>baseRect.xMax
            ||DragSlot.Inst.transform.localPosition.y<baseRect.yMin
            || DragSlot.Inst.transform.localPosition.y > baseRect.yMax)
        {
            if(DragSlot.Inst.dragSlot!= null)
            {
                theInputNumber.Call();
            }
        }
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
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Highlight(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Highlight(false);
    }
}
