using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Inst;

    public RPGPlayer theRPGPlayer;

    [SerializeField]
    Item FirstItem;
    [SerializeField]
    Item SecondItem;

    public float MaxHP;
    public float HP;
    public float MaxMP;
    public float MP;
    public float MaxEXP;
    public float EXP;
    public int LV;
    public float AP;
    public int Gold;
    public Sword sword;
    public Quest quest;

    public List<SkillData> skills;
    public List<SkillData> SkillSlotDatas;

    public List<Item> items;
    public List<int> itemsCount;

    public int questi;

    public bool IsFinishQuest = false;

    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }
        Inst = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        InventoryManager.Inst.invenSlots[0].AddItem(FirstItem);
        InventoryManager.Inst.invenSlots[1].AddItem(SecondItem);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (ActionController.Inst != null)
        {
            theRPGPlayer = ActionController.Inst.GetComponent<RPGPlayer>();
        }

        if (theRPGPlayer != null)
        {
            theRPGPlayer.myStat.maxHp = MaxHP;
            theRPGPlayer.myStat.HP = HP;
            theRPGPlayer.myStat.maxMp = MaxMP;
            theRPGPlayer.myStat.MP = MP;
            theRPGPlayer.myStat.maxExp = MaxEXP;
            theRPGPlayer.myStat.EXP = EXP;
            theRPGPlayer.Level = LV;
            theRPGPlayer.myStat.AP = AP;
            theRPGPlayer.mySword = sword;
            theRPGPlayer.quest = quest;
        }

        if(QuestManager.Inst != null)
        {
            QuestManager.Inst.i = questi;
        }
        else
        {
            if (questi != 0)
            {
                theRPGPlayer.i = questi;
            }
        }

        if (GameManager.Inst != null)
        {
            GameManager.Inst.Goldvalue = Gold;
        }
    }
}
