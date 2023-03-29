using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    GameObject theCharacter;

    public void OnPointerEnter(PointerEventData eventData)
    {
        theCharacter.GetComponent<Animator>().SetBool("Run", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (theCharacter.gameObject.IsDestroyed())
        {
            return;
        }
        theCharacter.GetComponent<Animator>().SetBool("Run", false);
    }
}
