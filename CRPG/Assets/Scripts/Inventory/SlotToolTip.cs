using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    GameObject go_Base;

    [SerializeField]
    TMP_Text ItemName;
    [SerializeField]
    TMP_Text ItemDesc;
    [SerializeField]
    TMP_Text ItemHowtoUsed;

    public void ShowToolTip(Item _item,Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f,
            0);
        go_Base.transform.position = _pos;

        ItemName.text = _item.itemName;
        ItemDesc.text = _item.itemDesc;

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

    public void HideToolTip()
    {
        go_Base.SetActive(false);
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
