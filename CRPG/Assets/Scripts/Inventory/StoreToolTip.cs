using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreToolTip : MonoBehaviour
{
    [SerializeField]
    protected GameObject go_Base;

    [SerializeField]
    protected TMP_Text ItemName;
    [SerializeField]
    protected TMP_Text ItemDesc;
    [SerializeField]
    protected TMP_Text Price;

    public virtual void ShowToolTip(Item _item, Vector3 _pos, int Pricenum)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f,
            0);
        go_Base.transform.position = _pos;

        ItemName.text = _item.itemName;
        ItemDesc.text = _item.itemDesc;

        Price.text = $"Price {Pricenum}";
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
