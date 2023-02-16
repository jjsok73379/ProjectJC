using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public static bool CharacterInfoActivated = false;
    public Transform HpBars;
    public int Goldvalue;
    int MaxGold;
    public TMP_Text GoldComma;
    public TMP_Text ZeroGold;
    [SerializeField]
    GameObject CharacterInfo;

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
        CharacterInfo.SetActive(false);
    }

    private void Update()
    {
        TryOpenCharacterInfo();
        if (Goldvalue == 0)
        {
            ZeroGold.text = Goldvalue.ToString();
        }
        else
        {
            GoldComma.text = GetThousandCommaText(Goldvalue).ToString();
        }
    }

    void TryOpenCharacterInfo()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CharacterInfoActivated = !CharacterInfoActivated;

            if (CharacterInfoActivated)
            {
                CharacterInfo.SetActive(true);
            }
            else
            {
                CharacterInfo.SetActive(false);
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
