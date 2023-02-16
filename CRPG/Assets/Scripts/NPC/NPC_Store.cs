using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Store : NPC
{
    string NPC_Typing = "오랜만이네 혹시 물건을 사러왔나?";
    [SerializeField]
    GameObject Store;
    [SerializeField]
    GameObject _buywindow;

    public bool IsStoreOpen = false;

    public override void ChangeState(STATE s)
    {
        if (myState == s) return;
        myState = s;

        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Talk:
                IsStoreOpen = true;
                Store.SetActive(false);
                _buywindow.SetActive(false);
                StartCoroutine(Typing(NPC_Typing));
                break;
            case STATE.Accept:
                Store.SetActive(true);
                break;
            case STATE.Refuse:
                IsStoreOpen = false;
                gameObject.SetActive(false);
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

    }

    public override void AcceptBtn()
    {
        ChangeState(STATE.Accept);
    }
}
