using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Item myData;
    [SerializeField]
    Image myImage;
    [SerializeField]
    int price;
    [SerializeField]
    ItemEffectDatabase theItemEffectDatabase;
    [SerializeField]
    GameObject myBuyWindow;
    [SerializeField]
    GameObject myBuyeqipWindow;
    [SerializeField]
    StoreInputNumber theStoreInputNumber;

    public int Price
    {
        get => price;
        set => price = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        myBuyWindow.SetActive(false); 
        myBuyeqipWindow.SetActive(false);
        myImage.sprite = myData.itemImage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        theStoreInputNumber.selectedSlot = eventData.pointerClick.transform.GetComponent<StoreSlot>();
        if(theStoreInputNumber.selectedSlot != null)
        {
            if (theStoreInputNumber.selectedSlot.myData.itemType == Item.ItemType.Equipment)
            {
                myBuyeqipWindow.SetActive(true);
                theStoreInputNumber.Call();
                myBuyWindow.SetActive(false);
            }
            else
            {
                myBuyWindow.SetActive(true);
                theStoreInputNumber.Call();
                myBuyeqipWindow.SetActive(false);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        theItemEffectDatabase.ShowStoreToolTip(myData, transform.position - new Vector3(250.0f, - 50.0f, 0), price);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theItemEffectDatabase.HideStoreToolTip();
    }
}
