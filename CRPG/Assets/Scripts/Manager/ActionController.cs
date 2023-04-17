using CombineRPG;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionController : MonoBehaviour
{
    public static ActionController Inst;

    [SerializeField]
    GameObject SceneStart;

    [SerializeField]
    float range; // ������ ������ ������ �ִ� �Ÿ�

    bool IsStore = false;
    bool IsRecovery = false;
    bool IsQuest = false;
    bool pickupActivated = false; // ������ ���� �����ҽ� True
    bool talkActivated = false;
    bool portalActivated = false;
    bool cautionActivated = false;

    RaycastHit hitInfo; // �浹ü ���� ����

    [SerializeField]
    LayerMask ItemMask; // Ư�� ���̾ ���� ������Ʈ�� ���ؼ��� ������ �� �־�� �Ѵ�.
    [SerializeField]
    LayerMask PortalMask;
    [SerializeField]
    LayerMask BossZoneMask;
    public LayerMask Store_NpcMask;
    public LayerMask Recovery_NpcMask;
    public LayerMask Quest_NpcMask;

    [SerializeField]
    TMP_Text actionText; // �ൿ�� ���� �� �ؽ�Ʈ
    public GameObject BossText;
    public GameObject Store_NPC_Text;
    public GameObject Recovery_NPC_Text;
    public GameObject Quest_NPC_Text;

    string NPC_Name;

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
        if (SceneStart != null)
        {
            StartCoroutine(SceneStartCo());
        }
        if (Store_NPC_Text != null)
        {
            Store_NPC_Text.SetActive(false);
        }
        if(Recovery_NPC_Text != null)
        {
            Recovery_NPC_Text.SetActive(false);
        }
        if(Quest_NPC_Text != null)
        {
            Quest_NPC_Text.SetActive(false);
        }
        if(BossText != null )
        {
            BossText.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pickupActivated && !talkActivated && !portalActivated && !cautionActivated)
        {
            actionText.gameObject.SetActive(false);
        }
        CheckItem();
        TryAction();
        NPC_Communication();
        CheckPortal();
        CheckBossZone();
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
            CheckBossZone();
            CanEnterBossZone();
        }
    }

    void CheckBossZone()
    {
        if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(1.0f, 3.0f, 1.0f), 1.0f, transform.forward, out hitInfo, range, BossZoneMask))
        {
            if (hitInfo.transform.CompareTag("BossZone") && !BossText.activeSelf)
            {
                BossZoneAppear();
            }
        }
        else
        {
            BossZoneDisappear();
        }
    }

    void CheckItem()
    {
        if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(1.0f, 3.0f, 1.0f), 1.0f, transform.forward, out hitInfo, range, ItemMask))
        {
            if (hitInfo.transform.CompareTag("Item"))
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
            if(hitInfo.transform.CompareTag("Store"))
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
            if(hitInfo.transform.CompareTag("Recovery"))
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
            if(hitInfo.transform.CompareTag("Quest"))
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
            if (hitInfo.transform.CompareTag("Portal"))
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
        actionText.gameObject.SetActive(true);
        actionText.text = "��Ż�� �̵��Ϸ��� (F)Ű�� ��������";
    }

    void NoPortal()
    {
        portalActivated = false;
    }

    void TalkWithNPC(string npcName)
    {
        if (!Store_NPC_Text.activeSelf && !Recovery_NPC_Text.activeSelf && !Quest_NPC_Text.activeSelf)
        {
            talkActivated = true;
            actionText.gameObject.SetActive(true);
            actionText.text = npcName + "��(��) ��ȭ�Ϸ��� (F)Ű�� ��������";
        }
    }

    void NobodyCanTalk() 
    {
        talkActivated = false;
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
    }

    void BossZoneAppear()
    {
        cautionActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = "�������� �����Ͻ÷��� (F)Ű�� ��������.";
    }

    void BossZoneDisappear()
    {
        cautionActivated = false;
    }

    void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                SoundManager.Inst.ButtonSound.Play();
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

    void CanEnterBossZone()
    {
        if (cautionActivated)
        {
            if (hitInfo.transform != null)
            {
                BossText.SetActive(true);
                BossZoneDisappear();
            }
        }
    }

    void CanTalk()
    {
        if (talkActivated)
        {
            if(hitInfo.transform != null)
            {
                SoundManager.Inst.ButtonSound.Play();
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
            SoundManager.Inst.ButtonSound.Play();
            DataManager.Inst.MaxHP = theRPGPlayer.myStat.maxHp;
            DataManager.Inst.HP = theRPGPlayer.myStat.HP;
            DataManager.Inst.MaxMP = theRPGPlayer.myStat.maxMp;
            DataManager.Inst.MP = theRPGPlayer.myStat.MP;
            DataManager.Inst.MaxEXP = theRPGPlayer.myStat.maxExp;
            DataManager.Inst.EXP = theRPGPlayer.myStat.EXP;
            DataManager.Inst.LV = theRPGPlayer.Level;
            DataManager.Inst.AP = theRPGPlayer.myStat.AP;
            DataManager.Inst.sword = theRPGPlayer.mySword;
            DataManager.Inst.quest = theRPGPlayer.quest;
            DataManager.Inst.Gold = GameManager.Inst.Goldvalue;
            DataManager.Inst.items.Clear();
            DataManager.Inst.itemsCount.Clear();
            for (int i = 0; i < InventoryManager.Inst.allSlots.Length; i++)
            {
                DataManager.Inst.items.Add(InventoryManager.Inst.allSlots[i].item);
                DataManager.Inst.itemsCount.Add(InventoryManager.Inst.allSlots[i].itemCount);
                InventoryManager.Inst.allSlots[i].item = null;
                InventoryManager.Inst.allSlots[i].itemCount = 0;
            }

            Destroy(SkillManager.Inst.AddedskillPanel);

            if(QuestManager.Inst != null)
            {
                Destroy(QuestManager.Inst.objQ2);
                DataManager.Inst.questi = QuestManager.Inst.i;
            }
            else
            {
                Destroy(theRPGPlayer.questobj);
                DataManager.Inst.questi = theRPGPlayer.i;
            }

            if (Physics.CapsuleCast(transform.position, transform.position + new Vector3(1.0f, 3.0f, 1.0f), 1.0f, transform.forward, out hitInfo, range, PortalMask))
            {
                if (hitInfo.transform.name == "ForestPortal")
                {
                    LoadManager.LoadScene(2);
                }
                else if (hitInfo.transform.name == "VillagePortal")
                {
                    LoadManager.LoadScene(1);
                }
                else if (hitInfo.transform.name == "BossPortal")
                {
                    LoadManager.LoadScene(3);
                }
            }
        }
    }

    public void GoTitle()
    {
        LoadManager.LoadScene(0);
        DataManager.Inst.items.Clear();
        DataManager.Inst.itemsCount.Clear();
        for (int i = 0; i < InventoryManager.Inst.allSlots.Length; i++)
        {
            InventoryManager.Inst.allSlots[i].item = null;
            InventoryManager.Inst.allSlots[i].itemCount = 0;
        }
        Destroy(SkillManager.Inst.AddedskillPanel);
        if (QuestManager.Inst != null)
        {
            Destroy(QuestManager.Inst.objQ2);
        }
        else
        {
            Destroy(theRPGPlayer.questobj);
        }
        DataManager.Inst.skills.Clear();
    }

    IEnumerator SceneStartCo()
    {
        SceneStart.SetActive(true);
        SceneStart.GetComponentInChildren<TMP_Text>().text = SceneManager.GetActiveScene().name;

        yield return new WaitForSeconds(1.0f);
        SceneStart.SetActive(false);
    }

    public IEnumerator WhenIventoryIsFull()
    {
        SoundManager.Inst.CannotSound.Play();
        itemText.gameObject.SetActive(true);
        itemText.text = "�������� �� á���ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNoItem()
    {
        SoundManager.Inst.CannotSound.Play();
        itemText.gameObject.SetActive(true);
        itemText.text = "����� �� �ִ� �������� �����ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenCannotSell()
    {
        SoundManager.Inst.CannotSound.Play();
        itemText.gameObject.SetActive(true);
        itemText.text = "�ش� �������� �����ϰ� �־\n�Ǹ��� �� �����ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNotCompleted()
    {
        SoundManager.Inst.CannotSound.Play();
        itemText.gameObject.SetActive(true);
        itemText.text = "����Ʈ ������\n������Ű�� ���Ͽ����ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenLessCount()
    {
        SoundManager.Inst.CannotSound.Play();
        itemText.gameObject.SetActive(true);
        itemText.text = "������ ������ �����մϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenDontHave()
    {
        SoundManager.Inst.CannotSound.Play();
        itemText.gameObject.SetActive(true);
        itemText.text = "�������� ������ ���� �ʽ��ϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNoMoney()
    {
        SoundManager.Inst.CannotSound.Play();
        itemText.gameObject.SetActive(true);
        itemText.text = "���� �����մϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator WhenNoMana()
    {
        SoundManager.Inst.CannotSound.Play();
        itemText.gameObject.SetActive(true);
        itemText.text = "������ �����մϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }

    public IEnumerator AlreadyHave()
    {
        SoundManager.Inst.CannotSound.Play();
        itemText.gameObject.SetActive(true);
        itemText.text = "�̹� ��ϵ� ��ų�Դϴ�.";

        yield return new WaitForSeconds(1.0f);
        itemText.gameObject.SetActive(false);
    }
}
