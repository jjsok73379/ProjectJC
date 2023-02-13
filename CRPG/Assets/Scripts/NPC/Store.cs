using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Store : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Item myData;
    [SerializeField]
    Image myImage;
    [SerializeField]
    GameObject myWindow;
    [SerializeField]
    Text text_Preview;
    [SerializeField]
    Text text_Input;
    [SerializeField]
    InputField if_text;
    [SerializeField]
    ItemEffectDatabase theItemEffectDatabase;
    [SerializeField]
    int price;

    bool IsOpen;
    int num;
    int maxnum = 99;

    // Start is called before the first frame update
    void Start()
    {
        myImage.sprite = myData.itemImage;
        if_text.text = "1";
        myWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OK();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
        }
    }

    public void OK()
    {
        if (text_Input.text != "")
        {
            if (CheckNumber(text_Input.text))
            {
                num = int.Parse(text_Input.text);
                if (num * price > GameManager.Inst.Goldvalue)
                {
                    num = GameManager.Inst.Goldvalue / price;
                }
                else
                {
                    num = 0;
                }
            }
            else
            {
                num = int.Parse(text_Input.text);
                if (num > maxnum)
                {
                    num = maxnum;
                }
                BuyItem(num);
            }
        }
    }

    public void Cancel()
    {
        myWindow.SetActive(false);
    }

    public void ClickPuls()
    {
        if (num * price > GameManager.Inst.Goldvalue || num > maxnum) return;
        num++;
    }

    public void ClickMinus()
    {
        if (num > 1) return;
        num--;
    }

    bool CheckNumber(string _argString)
    {
        char[] _tempCharArray = _argString.ToCharArray();
        bool isNumber = true;

        for (int i = 0; i < _tempCharArray.Length; i++)
        {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57)
            {
                continue;
            }
            isNumber = false;
        }
        return isNumber;
    }

    void BuyItem(int _num)
    {
        InventoryManager.Inst.AcquireItem(myData, _num);
        GameManager.Inst.Goldvalue -= num * price;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        myWindow.SetActive(true);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        theItemEffectDatabase.ShowStoreToolTip(myData, transform.position, price);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theItemEffectDatabase.HideStoreToolTip();
    }
}
