using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform HpBars;
    public int Goldvalue;
    int MaxGold;
    public TMP_Text GoldComma;
    public TMP_Text ZeroGold;

    public struct USERGOLD
    {
        [SerializeField] int gold;

        public int Gold
        {
            get => Gold;
            set
            {
                Gold = value;
            }
        }
    }

    private void Start()
    {
        MaxGold = 999999999;
    }

    private void Update()
    {
        if (Goldvalue == 0)
        {
            ZeroGold.text = Goldvalue.ToString();
        }
        else
        {
            GoldComma.text = GetThousandCommaText(Goldvalue).ToString();
        }
    }

    public void AddGold(int Price)
    {
        if (Goldvalue >= MaxGold)
        {
            Goldvalue = MaxGold;
        }
        else
        {
            Goldvalue += Price;
        }
    }

    public string GetThousandCommaText(int data)
    {
        return string.Format("{0:#,###}", data);
    }
}
