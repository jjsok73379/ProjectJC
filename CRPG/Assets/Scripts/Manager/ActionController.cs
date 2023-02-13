using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    float range; // ������ ������ ������ �ִ� �Ÿ�

    bool pickupActivated = false; // ������ ���� �����ҽ� True
    bool talkActivated = false; // ������ ���� �����ҽ� True

    RaycastHit hitInfo; // �浹ü ���� ����

    [SerializeField]
    LayerMask ItemMask; // Ư�� ���̾ ���� ������Ʈ�� ���ؼ��� ������ �� �־�� �Ѵ�.
    [SerializeField]
    LayerMask Store_NpcMask;
    [SerializeField]
    LayerMask Recovery_NpcMask;
    [SerializeField]
    LayerMask Quest_NpcMask;

    [SerializeField]
    TMP_Text actionText; // �ൿ�� ���� �� �ؽ�Ʈ
    [SerializeField]
    GameObject Store_NPC_Text;
    [SerializeField]
    GameObject Recovery_NPC_Text;
    [SerializeField]
    GameObject Quest_NPC_Text;

    //string Recovery_NPC_Typing = "���� ��ġ�̳׿�. ġ�Ḧ ������ ���̳���?";
    //string UnRecovery_NPC_Typing = "��ġ�� ���� �����. ġ�Ḧ ���� �� �����ϴ�.";
    public TMP_Text itemText; // �������� �� á�ٴ� ��� �޼����� ������ �ؽ�Ʈ

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            NPC_Communication();
        }
    }

    void CheckItem()
    {
        if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(1.0f, 3.0f, 1.0f), 1.0f, transform.forward, out hitInfo, range, ItemMask))
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

    void NPC_Communication()
    {
        if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(1.0f, 3.0f, 1.0f), 1.0f, transform.forward, out hitInfo, range, ItemMask))
        {
            if (hitInfo.transform.tag == "Store")
            {
                TalkWithNPC("��Ʈ��");
            }
        }
        else if (hitInfo.transform.tag == "Recovery")
        {
            TalkWithNPC("�Ƚ�");
        }
        else if (hitInfo.transform.tag == "Quest")
        {
            TalkWithNPC("ĳƮ ��");
        }
        else
        {
            NobodyCanTalk();
        }
    }

    void TalkWithNPC(string npcName)
    {
        talkActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = npcName + "��(��) ��ȭ�Ϸ��� (F)Ű�� ��������";
    }

    void NobodyCanTalk() 
    {
        talkActivated = false;
        actionText.gameObject.SetActive(false);
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
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    void CanTalk()
    {
        if (talkActivated)
        {
            if(hitInfo.transform != null)
            {

            }
        }
    }

    public IEnumerator WhenIventoryIsFull()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "�������� �� á���ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNoItem()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "����� �� �ִ� �������� �����ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }
}
