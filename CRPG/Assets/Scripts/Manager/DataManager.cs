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
    [SerializeField]
    Item ThirdItem;

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
    public SkillData[] SkillSlotDatas = new SkillData[4];

    public List<Item> items;
    public List<int> itemsCount;

    public int questi;

    public bool IsFinishQuest = false;

    public bool IsLoad = false;

    public bool IsQuesting = false;

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
        items[5] = FirstItem;
        items[0] = SecondItem;
        items[1] = ThirdItem;
        itemsCount[0] = 2;
        itemsCount[1] = 2;
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
            if (IsLoad)
            {
                SaveAndLoad.Inst.LoadData();
                IsLoad = false;
            }
            else
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
                GameManager.Inst.Goldvalue = Gold;
            }
        }
        if (questi != 0)
        {
            theRPGPlayer.i = questi;
        }
    }
}
