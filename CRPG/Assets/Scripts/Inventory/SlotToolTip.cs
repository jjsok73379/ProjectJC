using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotToolTip : StoreToolTip
{
    [SerializeField]
    TMP_Text ItemHowtoUsed;

    public override void ShowToolTip(Item _item,Vector3 _pos, int Pricenum)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            ItemHowtoUsed.text = "�� Ŭ�� - ����";
        }
        else if (_item.itemType == Item.ItemType.Potion)
        {
            ItemHowtoUsed.text = "�� Ŭ�� - ���ǻ��";
        }
        else if (_item.itemType == Item.ItemType.SkillBook)
        {
            ItemHowtoUsed.text = "�� Ŭ�� - ��ų���";
        }
        else
        {
            ItemHowtoUsed.text = "";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        HideToolTip();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
