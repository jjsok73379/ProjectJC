using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    float range; // 아이템 습득이 가능한 최대 거리

    bool pickupActivated = false; // 아이템 습득 가능할시 True
    bool talkActivated = false; // 아이템 습득 가능할시 True

    RaycastHit hitInfo; // 충돌체 정보 저장

    [SerializeField]
    LayerMask ItemMask; // 특정 레이어를 가진 오브젝트에 대해서만 습득할 수 있어야 한다.
    [SerializeField]
    LayerMask Store_NpcMask;
    [SerializeField]
    LayerMask Recovery_NpcMask;
    [SerializeField]
    LayerMask Quest_NpcMask;

    [SerializeField]
    TMP_Text actionText; // 행동을 보여 줄 텍스트
    [SerializeField]
    GameObject Store_NPC_Text;
    [SerializeField]
    GameObject Recovery_NPC_Text;
    [SerializeField]
    GameObject Quest_NPC_Text;

    //string Recovery_NPC_Typing = "많이 다치셨네요. 치료를 받으로 오셨나요?";
    //string UnRecovery_NPC_Typing = "다치신 곳이 없어요. 치료를 받을 수 없습니다.";
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
                TalkWithNPC("반트너");
            }
        }
        else if (hitInfo.transform.tag == "Recovery")
        {
            TalkWithNPC("픽시");
        }
        else if (hitInfo.transform.tag == "Quest")
        {
            TalkWithNPC("캐트 시");
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
        actionText.text = npcName + "와(과) 대화하려면 (F)키를 누르세요";
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
