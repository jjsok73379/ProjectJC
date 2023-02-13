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
            ItemHowtoUsed.text = "우 클릭 - 장착";
        }
        else if (_item.itemType == Item.ItemType.Potion)
        {
            ItemHowtoUsed.text = "우 클릭 - 포션사용";
        }
        else if (_item.itemType == Item.ItemType.SkillBook)
        {
            ItemHowtoUsed.text = "우 클릭 - 스킬등록";
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
