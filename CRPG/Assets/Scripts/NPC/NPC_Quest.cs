using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Quest : NPC
{
    public override void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Talk:
                break;
            case STATE.Accept:
                break;
            case STATE.Refuse:
                gameObject.SetActive(false);
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Talk:
                break;
            case STATE.Accept:
                break;
            case STATE.Refuse:
                break;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    public override void AcceptBtn()
    {
        ChangeState(STATE.Accept);
    }
}
