using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenManager : Singleton<OpenManager>
{
    public static bool inventoryActivated = false; // 인벤토리 활성화 여부. true가 되면 카메라 움직임과 다른 입력을 막을 것이다.
    public static bool CharacterInfoActivated = false;
    public static bool CombineActivated = false;
    public static bool QuestActivated = false;
    public static bool MenuActivated = false;
    public static bool SettingActivated = false;
    public static bool SaveLoadActivated = false;
    public bool SkillActivated = false;

    [SerializeField]
    GameObject CharacterInfo;

    public GameObject CombineWindow;
    public GameObject SkillMenu;
    public GameObject go_Quest;
    public GameObject go_Inventory;
    public GameObject go_Menu;
    public GameObject go_Setting;

    bool isActivating = false;

    // Start is called before the first frame update
    void Start()
    {
        CharacterInfo.SetActive(false);
        SkillMenu.SetActive(false);
        CombineWindow.SetActive(false);
        go_Quest.SetActive(false);
        go_Inventory.SetActive(false);
        go_Menu.SetActive(false);
        go_Setting.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TryOpenInventory();
        TryOpenCharacterInfo();
        TryOpenSkill();
        TryOpenCombine();
        TryOpenQuestUI();
        TryOpenSettingMenu();
    }

    public void OpenMenu()
    {
        MenuActivated = !MenuActivated;
        go_Menu.SetActive(MenuActivated);
    }

    public void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;
            go_Inventory.SetActive(inventoryActivated);
        }

        if (inventoryActivated)
        {
            isActivating = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                inventoryActivated = false;
                go_Inventory.SetActive(false);
            }
        }
        else 
        {
            for (int i = 0; i < InventoryManager.Inst.allSlots.Length; i++)
            {
                if (!InventoryManager.Inst.allSlots[i].isQuickSlot)
                {
                    if (!inventoryActivated)
                    {
                        ItemEffectDatabase.Inst.HideToolTip();
                    }
                }
            }
        }
        if (!go_Inventory.activeSelf)
        {
            isActivating = false;
        }
    }


    public void TryOpenCharacterInfo()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CharacterInfoActivated = !CharacterInfoActivated;
            CharacterInfo.SetActive(CharacterInfoActivated);
        }
        if (CharacterInfoActivated)
        {
            isActivating = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CharacterInfoActivated = false;
                CharacterInfo.SetActive(false);
            }
        }
        if (!CharacterInfo.activeSelf)
        {
            isActivating = false;
        }
    }

    public void TryOpenSkill()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            SkillActivated = !SkillActivated;
            SkillMenu.SetActive(SkillActivated);
        }

        if (SkillActivated)
        {
            isActivating = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SkillActivated = false;
                SkillMenu.SetActive(false);
            }
        }
        else
        { 
            SkillManager.Inst.ReinforceOpen = false;
            SkillManager.Inst.ReinforceWindow.SetActive(false);
        }
        if (!SkillMenu.activeSelf)
        {
            isActivating = false;
        }
    }
    
    public void TryOpenCombine()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CombineActivated = !CombineActivated;
            CombineWindow.SetActive(CombineActivated);
        }

        if (CombineActivated)
        {
            isActivating = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CombineActivated = false;
                CombineWindow.SetActive(false);
            }
        }
        else
        {
            SkillManager.Inst.myCombinedSkillText.SetActive(true);
        }
        if(!CombineWindow.activeSelf)
        {
            isActivating = false;
        }
    }

    public void TryOpenQuestUI()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuestActivated = !QuestActivated;
            go_Quest.SetActive(QuestActivated);
        }

        if (QuestActivated)
        {
            isActivating = true;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuestActivated = false;
                go_Quest.SetActive(false);
            }
        }
        else
        {
            if (QuestManager.Inst.objQ2 != null)
            {
                QuestManager.Inst.objQ2.GetComponent<QuestInfo>().myInfo.SetActive(false);
            }
        }
        if(!go_Quest.activeSelf)
        {
            isActivating = false;
        }
    }

    public  void TryOpenSettingMenu()
    {
        if (!isActivating)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SettingActivated = !SettingActivated;
                go_Setting.SetActive(SettingActivated);
            }
            if (SettingActivated)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}
