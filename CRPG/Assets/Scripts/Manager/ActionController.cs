using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionController : MonoBehaviour
{
    public static ActionController Inst;
    [SerializeField]
    float range; // 아이템 습득이 가능한 최대 거리

    bool IsStore = false;
    bool IsRecovery = false;
    bool IsQuest = false;
    bool pickupActivated = false; // 아이템 습득 가능할시 True
    bool talkActivated = false;
    bool portalActivated = false;

    RaycastHit hitInfo; // 충돌체 정보 저장

    [SerializeField]
    LayerMask ItemMask; // 특정 레이어를 가진 오브젝트에 대해서만 습득할 수 있어야 한다.
    [SerializeField]
    LayerMask PortalMask;
    public LayerMask Store_NpcMask;
    public LayerMask Recovery_NpcMask;
    public LayerMask Quest_NpcMask;

    [SerializeField]
    TMP_Text actionText; // 행동을 보여 줄 텍스트
    [SerializeField]
    TMP_Text portalText;
    public GameObject Store_NPC_Text;
    public GameObject Recovery_NPC_Text;
    public GameObject Quest_NPC_Text;

    string NPC_Name;

    //string Recovery_NPC_Typing = "많이 다치셨네요. 치료를 받으로 오셨나요?";
    //string UnRecovery_NPC_Typing = "다치신 곳이 없어요. 치료를 받을 수 없습니다.";
    public TMP_Text itemText; // 아이템이 꽉 찼다는 경고 메세지를 보여줄 텍스트

    InventoryManager theInventory;
    RPGPlayer theRPGPlayer;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        theRPGPlayer = GetComponent<RPGPlayer>();
        theInventory = InventoryManager.Inst;
        Store_NPC_Text.SetActive(false);
        Recovery_NPC_Text.SetActive(false);
        Quest_NPC_Text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        TryAction();
        NPC_Communication();
        CheckPortal();
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
            CanTalk();
            CheckPortal();
            CanUsePortal();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
        if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(1.0f, 3.0f, 1.0f), 1.0f, transform.forward, out hitInfo, range, Store_NpcMask))
        {
            if(hitInfo.transform.tag == "Store")
            {
                IsStore = true;
                IsRecovery = false;
                IsQuest = false;
                NPC_Name = "반트너";
                TalkWithNPC(NPC_Name);
            }
        }
        else if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(1.0f, 3.0f, 1.0f), 1.0f, transform.forward, out hitInfo, range, Recovery_NpcMask))
        {
            if(hitInfo.transform.tag == "Recovery")
            {
                IsStore = false;
                IsRecovery = true;
                IsQuest = false;
                NPC_Name = "픽시";
                TalkWithNPC(NPC_Name);
            }

        }
        else if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(1.0f, 3.0f, 1.0f), 1.0f, transform.forward, out hitInfo, range, Quest_NpcMask))
        {
            if(hitInfo.transform.tag == "Quest")
            {
                IsStore = false;
                IsRecovery = false;
                IsQuest = true;
                NPC_Name = "캐트 시";
                TalkWithNPC(NPC_Name);
            }
        }
        else
        {
            NobodyCanTalk();
        }
    }

    void CheckPortal()
    {
        if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(1.0f, 3.0f, 1.0f), 1.0f, transform.forward, out hitInfo, range, PortalMask))
        {
            if (hitInfo.transform.tag == "Portal")
            {
                FrontPortal();
            }
        }
        else
        {
            NoPortal();
        }
    }

    void FrontPortal()
    {
        portalActivated = true;
        portalText.gameObject.SetActive(true);
        portalText.text = "포탈로 이동하려면 (F)키를 누르세요";
    }

    void NoPortal()
    {
        portalActivated = false;
        portalText.gameObject.SetActive(false);
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
                if (theRPGPlayer.quest.isActive)
                {
                    theRPGPlayer.quest.goal.ItemCollected(hitInfo.transform.gameObject);
                }
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
                if (IsStore)
                {
                    Store_NPC_Text.SetActive(true);
                    Store_NPC_Text.GetComponent<NPC_Store>().ChangeState(NPC.STATE.Talk);
                }
                else if (IsRecovery)
                {
                    Recovery_NPC_Text.SetActive(true);
                    Recovery_NPC_Text.GetComponent<NPC_Recovery>().ChangeState(NPC.STATE.Talk);
                }
                else if (IsQuest)
                {
                    Quest_NPC_Text.SetActive(true);
                    Quest_NPC_Text.GetComponent<NPC_Quest>().ChangeState(NPC.STATE.Talk);
                }
                NobodyCanTalk();
            }
        }
    }

    void CanUsePortal()
    {
        if (portalActivated)
        {
            SceneManager.LoadScene("LoadingScene");
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

    public IEnumerator WhenCannotSell()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "해당 아이템을 장착하고 있어서\n판매할 수 없습니다.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNotCompleted()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "퀘스트 조건을\n충족시키지 못하였습니다.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenLessCount()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "아이템 수량이 부족합니다.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenDontHave()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "아이템을 가지고 있지 않습니다.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNoMoney()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "돈이 부족합니다.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }
    public IEnumerator WhenNoMana()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "마나가 부족합니다.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }
}
