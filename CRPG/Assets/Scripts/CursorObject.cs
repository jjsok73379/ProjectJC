using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CursorManager.CursorType cursorType;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cursorType != CursorManager.CursorType.Magic)
        {
            CursorManager.Inst.SetActiveCursorType(cursorType);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Inst.SetActiveCursorType(CursorManager.CursorType.Arrow);
    }
}
