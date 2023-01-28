using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupManager : MonoBehaviour
{
    public static PopupManager Inst = null;
    public GameObject noTouch = null;
    UnityAction allClose = null;
    Stack<UnityAction> myCloses = new Stack<UnityAction>();
    private void Awake()
    {
        Inst = this;
    }
    
    public void AddPopup(string title, string content)
    {
        noTouch.SetActive(true);
        noTouch.transform.SetAsLastSibling();
        Vector2 pos = new Vector2(Screen.width, Screen.height) * 0.5f;
        GameObject obj = Instantiate(Resources.Load("Prefabs/UI/PopUp"), pos, Quaternion.identity, transform) as GameObject;
        PopUp scp = obj.GetComponent<PopUp>();
        scp.Initialize(title, content);
        allClose += scp.OnClose;
        myCloses.Push(scp.OnClose);
    }
    public void ClosePopup(PopUp scp)
    {
        allClose -= scp.OnClose;
        if (myCloses.Count > 0 && myCloses.Peek() == scp.OnClose)
        {
            myCloses.Pop();
        }

        if (myCloses.Count == 0)
        {
            noTouch.SetActive(false);
        }
        else
        {
            noTouch.transform.SetSiblingIndex(noTouch.transform.GetSiblingIndex() - 1);
        }        
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            noTouch.SetActive(false);
            allClose?.Invoke();
            myCloses.Clear();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(myCloses.Count > 0) myCloses.Pop().Invoke();
        }
    }
}
