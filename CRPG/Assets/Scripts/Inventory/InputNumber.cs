using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputNumber : MonoBehaviour
{
    public static InputNumber Inst;
    private void Awake()
    {
        Inst = this;
    }
    bool activated = false;

    public bool IsDrop = false;
    public Item DropItemData;

    [SerializeField]
    Text text_Preview;
    [SerializeField]
    Text text_Input;
    [SerializeField]
    InputField if_text;

    [SerializeField]
    GameObject go_Base;

    // Start is called before the first frame update
    void Start()
    {
        go_Base.SetActive(false);
        text_Preview.text = "1";
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            if (Input.GetKeyDown(KeyCode.Return))
                OK();
            else if (Input.GetKeyDown(KeyCode.Escape))
                Cancel();
        }
    }

    public void Call()
    {
        go_Base.SetActive(true);
        activated = true;
        if_text.text = "";
        text_Preview.text = DragSlot.Inst.dragSlot.itemCount.ToString();
    }

    public void Cancel()
    {
        SoundManager.Inst.ButtonSound.Play();
        activated = false;
        DragSlot.Inst.SetColor(0);
        go_Base.SetActive(false);
        DragSlot.Inst.dragSlot = null;
    }

    public void OK()
    {
        SoundManager.Inst.ButtonSound.Play();
        int num;
        if (text_Input.text != "")
        {
            DragSlot.Inst.SetColor(0);
            if (CheckNumber(text_Input.text))
            {
                num = int.Parse(text_Input.text);
                if (num > DragSlot.Inst.dragSlot.itemCount)
                {
                    num = DragSlot.Inst.dragSlot.itemCount;
                }
            }
            else
            {
                num = int.Parse(text_Preview.text);
            }
            StartCoroutine(DropItemCoroutone(num));
        }
    }

    IEnumerator DropItemCoroutone(int _num)
    {
        IsDrop = true;

        if (DragSlot.Inst.dragSlot.item.itemPrefab != null)
        {
            DropItemData = DragSlot.Inst.dragSlot.item;
            DropItemData.DropCount = _num;
            Instantiate(DragSlot.Inst.dragSlot.item.itemPrefab,
                ActionController.Inst.transform.position + ActionController.Inst.transform.forward,
                Quaternion.identity);
        }
        DragSlot.Inst.dragSlot.SetSlotCount(-_num);

        yield return new WaitForSeconds(0.05f);

        DragSlot.Inst.dragSlot = null;
        go_Base.SetActive(false);
        activated = false;
        IsDrop = false;
    }

    bool CheckNumber(string _argString)
    {
        char[] _tempCharArray = _argString.ToCharArray();
        bool isNumber = true;

        for(int i=0;i<_tempCharArray.Length;i++)
        {
            if (_tempCharArray[i] >= 48 && _tempCharArray[i] <= 57)
            {
                continue;
            }
            isNumber = false;
        }
        return isNumber;
    }
}
