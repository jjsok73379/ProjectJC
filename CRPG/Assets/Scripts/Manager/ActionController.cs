using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    float range; // 아이템 습득이 가능한 최대 거리

    bool pickupActivated = false; // 아이템 습득 가능할시 True

    RaycastHit hitInfo; // 충돌체 정보 저장

    [SerializeField]
    LayerMask layerMask; // 특정 레이어를 가진 오브젝트에 대해서만 습득할 수 있어야 한다.

    [SerializeField]
    TMP_Text actionText; // 행동을 보여 줄 텍스트
    
    public TMP_Text itemText; // 아이템이 꽉 찼다는 경고 메세지를 보여줄 텍스트

    InventoryManager theInventory;

    private void Start()
    {
        theInventory = InventoryManager.Inst;
    }

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
    }

    void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CheckItem();
            CanPickUp();
        }
    }

    void CheckItem()
    {
        if(Physics.SphereCast(transform.position, 1.0f,transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else
        {
            ItemInfoDisappear();
        }
    }

    void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<PickupItem>().item.itemName + " 획득 " + "<color=yellow>" + "(Z)" + "</color>";
    }

    void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<PickupItem>().item.itemName + " 획득했습니다. "); // 인벤토리 넣기
                theInventory.AcquireItem(hitInfo.transform.GetComponent<PickupItem>().item);
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    public IEnumerator WhenIventoryIsFull()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "아이템이 꽉 찼습니다.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNoItem()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "사용할 수 있는 아이템이 없습니다.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }
}
