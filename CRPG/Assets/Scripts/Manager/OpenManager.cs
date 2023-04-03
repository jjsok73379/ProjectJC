using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class OpenManager : MonoBehaviour
{
    public static OpenManager Inst;

    public static bool inventoryActivated = false; // 인벤토리 활성화 여부. true가 되면 카메라 움직임과 다른 입력을 막을 것이다.
    public static bool CharacterInfoActivated = false;
    public static bool CombineActivated = false;
    public static bool QuestActivated = false;
    public static bool MenuActivated = false;
    public static bool SettingActivated = false;
    public static bool SoundActivated = false;
    public bool SkillActivated = false;

    [SerializeField]
    GameObject CharacterInfo;
    [SerializeField]
    BossZone theBossZone;
    [SerializeField]
    PlayableDirector thePlayableDirector;
    [SerializeField]
    BossDragon theBossDragon;

    public GameObject CombineWindow;
    public GameObject SkillMenu;
    public GameObject go_Quest;
    public GameObject go_Inventory;
    public GameObject go_Menu;
    public GameObject go_Setting;
    public GameObject go_Sound;

    public bool IsActive = false;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(SceneManager.GetActiveScene().name != "Title")
        {
            CharacterInfo.SetActive(false);
            SkillMenu.SetActive(false);
            CombineWindow.SetActive(false);
            go_Quest.SetActive(false);
            go_Inventory.SetActive(false);
            go_Menu.SetActive(false);
            go_Setting.SetActive(false);
        }
        go_Sound.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!inventoryActivated && !CharacterInfoActivated && !CombineActivated && !MenuActivated && !SkillActivated)
        {
            IsActive = false;
            if (QuestManager.Inst != null)
            {
                if (QuestActivated)
                {
                    IsActive = true;
                }
                else
                {
                    IsActive = false;
                }
            }
        }
        else
        {
            IsActive = true;
        }
        if(go_Inventory != null)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                SoundManager.Inst.OpenWindowSound.Play();
                TryOpenInventory();
            }
            if (inventoryActivated)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SoundManager.Inst.OpenWindowSound.Play();
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
        }

        if(CharacterInfo != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SoundManager.Inst.OpenWindowSound.Play();
                TryOpenCharacterInfo();
            }
            if (CharacterInfoActivated)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SoundManager.Inst.OpenWindowSound.Play();
                    CharacterInfoActivated = false;
                    CharacterInfo.SetActive(false);
                }
            }
        }

        if(SkillMenu != null)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                SoundManager.Inst.OpenWindowSound.Play();
                TryOpenSkill();
            }
            if (SkillActivated)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SoundManager.Inst.OpenWindowSound.Play();
                    SkillActivated = false;
                    SkillMenu.SetActive(false);
                }
            }
            else
            {
                SkillManager.Inst.ReinforceOpen = false;
                SkillManager.Inst.ReinforceWindow.SetActive(false);
            }
        }

        if(CombineWindow != null)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                SoundManager.Inst.OpenWindowSound.Play();
                TryOpenCombine();
            }
            if (CombineActivated)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SoundManager.Inst.OpenWindowSound.Play();
                    CombineActivated = false;
                    CombineWindow.SetActive(false);
                }
            }
            else
            {
                SkillManager.Inst.myCombinedSkillText.SetActive(true);
            }
        }

        if(go_Quest != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SoundManager.Inst.OpenWindowSound.Play();
                TryOpenQuestUI();
            }
            if (QuestActivated)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SoundManager.Inst.OpenWindowSound.Play();
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
        }

        if(go_Setting != null)
        {
            if (!IsActive)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    SoundManager.Inst.OpenWindowSound.Play();
                    TryOpenSettingMenu();
                }
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

        if (theBossZone != null && thePlayableDirector != null && theBossDragon != null)
        {
            if (theBossZone.IsEnter)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    thePlayableDirector.Stop();
                    theBossDragon.gameObject.transform.position = new Vector3(34.8f, 2.1f, 75.3f);
                    theBossDragon.myHPSlider.gameObject.SetActive(true);
                    theBossZone.IsEnter = false;
                }
            }
        }
    }

    public void OpenMenu()
    {
        MenuActivated = !MenuActivated;
        go_Menu.SetActive(MenuActivated);
    }

    public void TryOpenInventory()
    {
        inventoryActivated = !inventoryActivated;
        go_Inventory.SetActive(inventoryActivated);
    }


    public void TryOpenCharacterInfo()
    {
        CharacterInfoActivated = !CharacterInfoActivated;
        CharacterInfo.SetActive(CharacterInfoActivated);
    }

    public void TryOpenSkill()
    {
        SkillActivated = !SkillActivated;
        SkillMenu.SetActive(SkillActivated);
    }

    public void TryOpenCombine()
    {
        CombineActivated = !CombineActivated;
        CombineWindow.SetActive(CombineActivated);
    }

    public void TryOpenQuestUI()
    {
        QuestActivated = !QuestActivated;
        go_Quest.SetActive(QuestActivated);
    }

    public void TryOpenSettingMenu()
    {
        SettingActivated = !SettingActivated;
        go_Setting.SetActive(SettingActivated);
    }

    public void TryOpenSoundWindow()
    {
        SoundActivated = !SoundActivated;
        go_Sound.SetActive(SoundActivated);
    }
}
