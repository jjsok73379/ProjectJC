using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreInputNumber : MonoBehaviour
{
    bool activated = false;

    [SerializeField] 
    TMP_InputField text_Input;
    [SerializeField]
    BuyWindow theBuyWindow;

    public int num = 1;
    int maxnum = 99;

    public StoreSlot selectedSlot;

    void Update()
    {
        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                OK();
            else if (Input.GetKeyDown(KeyCode.Escape))
                Cancel();
        }
        text_Input.text = num.ToString();
        theBuyWindow.Text_Buy(selectedSlot.myData.itemName, selectedSlot.Price * num);
    }

    void BuyItem(int _num)
    {
        InventoryManager.Inst.AcquireItem(selectedSlot.myData, _num);
        GameManager.Inst.Goldvalue -= num * selectedSlot.Price;
    }

    public void Call()
    {
        activated = true;
        num = 1;
    }

    public void Cancel()
    {
        activated = false;
        transform.parent.gameObject.SetActive(false);
        num = 1;
    }

    public void OK()
    {
        if (text_Input.text != "")
        {
            if (GameManager.Inst.Goldvalue < selectedSlot.Price) return;
            BuyItem(num);
            transform.parent.gameObject.SetActive(false);
            num = 1;
        }
    }

    public void ClickPuls()
    {
        if (num * selectedSlot.Price > GameManager.Inst.Goldvalue)
        {
            return;
        }
        else if (num == maxnum)
        {
            num = 1;
        }
        else
        {
            num++;
        }
    }

    public void ClickMinus()
    {
        if (num <= 1)
        {
            if (GameManager.Inst.Goldvalue >= maxnum * selectedSlot.Price)
            {
                num = 99;
            }
            else
            {
                return;
            }
        }
        else
        {

            num--;
        }
    }

    public void ClickPulsTen()
    {
        if (num * selectedSlot.Price > GameManager.Inst.Goldvalue)
        {
            return;
        }
        else if (num == maxnum)
        {
            num = 1;
        }
        else if (num + 10 >= maxnum)
        {
            num = maxnum;
        }
        else
        {
            num += 10;
        }
    }

    public void ClickMinusTen()
    {
        if (num <= 1)
        {
            if (GameManager.Inst.Goldvalue >= maxnum * selectedSlot.Price)
            {
                num = 99;
            }
            else
            {
                return;
            }
        }
        else if (num - 10 <= 1)
        {
            num = 1;
        }
        else
        {

            num -= 10;
        }
    }
}
