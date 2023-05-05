using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text myCount;
    public SkillData myData;

    [SerializeField]
    GameObject go_Base;

    [SerializeField]
    TMP_Text ItemName;
    [SerializeField]
    TMP_Text ItemDesc;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        go_Base?.SetActive(false);
    }

    void Start()
    {
        go_Base.SetActive(false);
    }

    void ShowToolTip()
    {
        go_Base.SetActive(true);
        go_Base.transform.position = transform.position + new Vector3(0, 150, 0);

        ItemName.text = "½ºÅ³ºÏ : " + myData.SkillName;
        ItemDesc.text = myData.myInfo;
    }
}
