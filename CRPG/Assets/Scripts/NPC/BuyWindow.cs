using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class BuyWindow : MonoBehaviour
{
    [SerializeField]
    TMP_Text BuyText;
    [SerializeField]
    TMP_Text Price;

    public void Text_Buy(string itemName, int price)
    {
        BuyText.text = itemName + "��(��)\n�����Ͻðڽ��ϱ�?";
        Price.text = "Price : " + price.ToString();
    }
}
