using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Quest : NPC
{
    string NPC_Typing = "�� �� ���ʹ޶��...";
    [SerializeField]
    GameObject QuestWindow;

    public override void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Talk:
                QuestWindow.SetActive(false);
                StartCoroutine(Typing(NPC_Typing));
                break;
            case STATE.Accept:
                QuestWindow.SetActive(true);
                break;
            case STATE.Refuse:
                gameObject.SetActive(false);
                break;
        }
    }
}
