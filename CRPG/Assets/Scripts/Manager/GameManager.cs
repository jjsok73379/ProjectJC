using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

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

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        MaxGold = 999999999;
    }

    private void Update()
    {
        if (Goldvalue == 0)
        {
            if(ZeroGold != null)
            {
                ZeroGold.text = Goldvalue.ToString();
            }
        }
        else
        {
            if(GoldComma != null)
            {
                GoldComma.text = GetThousandCommaText(Goldvalue).ToString();
            }
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
