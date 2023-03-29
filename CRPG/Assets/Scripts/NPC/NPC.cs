using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum STATE
    {
        Create, Talk, Accept ,Refuse
    }

    public STATE myState = STATE.Create;

    public virtual void ChangeState(STATE s)
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
                break;
        }
    }
    [SerializeField]
    protected TMP_Text Communication;

    protected IEnumerator Typing(string Typing)
    {
        for (int i = 0; i <= Typing.Length; ++i)
        {
            SoundManager.Inst.TypingSound.Play();
            Communication.text = Typing.Substring(0, i);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    public virtual void AcceptBtn()
    {
        ChangeState(STATE.Accept);
    }

    public void RefuseBtn()
    {
        ChangeState(STATE.Refuse);
    }
}
