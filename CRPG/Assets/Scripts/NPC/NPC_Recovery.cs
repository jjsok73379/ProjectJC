using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_Recovery : NPC
{
    string NPC_Typing = "ġ�ᰡ �ʿ��ϽŰ���?";
    int RecoveryPrice = 0;
    [SerializeField]
    RPGPlayer thePlayer;
    [SerializeField]
    GameObject go_Recovery;
    [SerializeField]
    TMP_Text CostText;

    public override void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Talk:
                go_Recovery.SetActive(false);
                if (thePlayer.myStat.HP == thePlayer.myStat.maxHp && thePlayer.myStat.MP == thePlayer.myStat.maxMp)
                {
                    RecoveryPrice = 0;
                }
                else
                {
                    RecoveryPrice = 150;
                }
                CostText.text = $"{RecoveryPrice} ��带\n������\nġ�� �����ðڽ��ϱ�?";
                StartCoroutine(Typing(NPC_Typing));
                break;
            case STATE.Accept:
                go_Recovery.SetActive(true);
                break;
            case STATE.Refuse:
                gameObject.SetActive(false);
                break;
        }
    }

    public override void AcceptBtn()
    {
        ChangeState(STATE.Accept);
    }

    public void RecoveryBtn()
    {
        GameManager.Inst.Goldvalue -= RecoveryPrice;
        thePlayer.myStat.HP = thePlayer.myStat.maxHp;
        thePlayer.myStat.MP = thePlayer.myStat.maxMp;
        gameObject.SetActive(false);
    }
}
