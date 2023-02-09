using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    float range; // ������ ������ ������ �ִ� �Ÿ�

    bool pickupActivated = false; // ������ ���� �����ҽ� True

    RaycastHit hitInfo; // �浹ü ���� ����

    [SerializeField]
    LayerMask layerMask; // Ư�� ���̾ ���� ������Ʈ�� ���ؼ��� ������ �� �־�� �Ѵ�.

    [SerializeField]
    TMP_Text actionText; // �ൿ�� ���� �� �ؽ�Ʈ

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
        actionText.text = hitInfo.transform.GetComponent<PickupItem>().item.itemName + " ȹ�� " + "<color=yellow>" + "(Z)" + "</color>";
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
                Debug.Log(hitInfo.transform.GetComponent<PickupItem>().item.itemName + " ȹ���߽��ϴ�. "); // �κ��丮 �ֱ�
                theInventory.AcquireItem(hitInfo.transform.GetComponent<PickupItem>().item);
                hitInfo.transform.gameObject.SetActive(false);
                ItemInfoDisappear();
            }
        }
    }
}
