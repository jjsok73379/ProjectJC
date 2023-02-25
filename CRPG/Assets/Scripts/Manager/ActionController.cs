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
    float range; // ������ ������ ������ �ִ� �Ÿ�

    bool IsStore = false;
    bool IsRecovery = false;
    bool IsQuest = false;
    bool pickupActivated = false; // ������ ���� �����ҽ� True
    bool talkActivated = false;
    bool portalActivated = false;

    RaycastHit hitInfo; // �浹ü ���� ����

    [SerializeField]
    LayerMask ItemMask; // Ư�� ���̾ ���� ������Ʈ�� ���ؼ��� ������ �� �־�� �Ѵ�.
    [SerializeField]
    LayerMask PortalMask;
    public LayerMask Store_NpcMask;
    public LayerMask Recovery_NpcMask;
    public LayerMask Quest_NpcMask;

    [SerializeField]
    TMP_Text actionText; // �ൿ�� ���� �� �ؽ�Ʈ
    [SerializeField]
    TMP_Text portalText;
    public GameObject Store_NPC_Text;
    public GameObject Recovery_NPC_Text;
    public GameObject Quest_NPC_Text;

    string NPC_Name;

    //string Recovery_NPC_Typing = "���� ��ġ�̳׿�. ġ�Ḧ ������ ���̳���?";
    //string UnRecovery_NPC_Typing = "��ġ�� ���� �����. ġ�Ḧ ���� �� �����ϴ�.";
    public TMP_Text itemText; // �������� �� á�ٴ� ��� �޼����� ������ �ؽ�Ʈ

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
                NPC_Name = "��Ʈ��";
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
                NPC_Name = "�Ƚ�";
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
                NPC_Name = "ĳƮ ��";
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
        portalText.text = "��Ż�� �̵��Ϸ��� (F)Ű�� ��������";
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

    public IEnumerator WhenCannotSell()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "�ش� �������� �����ϰ� �־\n�Ǹ��� �� �����ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNotCompleted()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "����Ʈ ������\n������Ű�� ���Ͽ����ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenLessCount()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "������ ������ �����մϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenDontHave()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "�������� ������ ���� �ʽ��ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNoMoney()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "���� �����մϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }
    public IEnumerator WhenNoMana()
    {
        itemText.gameObject.SetActive(true);
        itemText.text = "������ �����մϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }
}
