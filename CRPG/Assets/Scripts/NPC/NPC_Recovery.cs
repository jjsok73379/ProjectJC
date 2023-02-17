using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC_Recovery : NPC
{
    string NPC_Typing = "치료가 필요하신가요?";
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
                CostText.text = $"{RecoveryPrice} 골드를\n지불해\n치료 받으시겠습니까?";
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
