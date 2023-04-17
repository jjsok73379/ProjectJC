using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CombineRPG;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class SaveData
{
    public string SceneName;
    public int SceneNum;

    public Vector3 playerPos;
    public Vector3 playerRot;
    public float playerHP;
    public float playerMaxHP;
    public float playerMP;
    public float playerMaxMP;
    public float playerEXP;
    public float playerMaxEXP;
    public int playerLV;
    public Quest playerQuest;
    public Sword mySword;

    public int Money;

    // ������ ����ȭ�� �Ұ���, ����ȭ�� �Ұ����� �ֵ��� �ִ�.
    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

    public List<SkillData> Datas = new List<SkillData>();

    public int questNum;
}

public class SaveAndLoad : Singleton<SaveAndLoad>
{
    public SaveData saveData = new SaveData();

    string SAVE_DATA_DIRECTORY; // ������ ���� ���
    string SAVE_FILENAME = "/SaveFile.txt"; // ���� �̸�

    public bool IsSaved = false;

    RPGPlayer theRPGPlayer; // �÷��̾��� ���� �������� ���� �ʿ�
    InventoryManager theInventoryManager; // �κ��丮 ���� �������� ���� �ʿ�
    SkillManager theSkillManager; // ��ų ���� �������� ���� �ʿ�

    // Start is called before the first frame update
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + $"/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // �ش� ��ΰ� �������� �ʴ´ٸ�
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // ���� ����
        }

        if(!File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            IsSaved = false;
        }
        else
        {
            IsSaved = true;
        }
    }

    public void ClickLoad()
    {
        string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
        saveData = JsonUtility.FromJson<SaveData>(loadJson);
    }

    public void SaveData()
    {
        IsSaved = true;

        theRPGPlayer = ActionController.Inst.GetComponent<RPGPlayer>();
        theInventoryManager = InventoryManager.Inst;
        theSkillManager = SkillManager.Inst;

        saveData.SceneName = SceneManager.GetActiveScene().name;

        // �÷��̾� ���� ����
        saveData.playerPos = theRPGPlayer.transform.position;
        saveData.playerRot = theRPGPlayer.transform.rotation.eulerAngles;
        saveData.playerHP = theRPGPlayer.myStat.HP;
        saveData.playerMaxHP = theRPGPlayer.myStat.maxHp;
        saveData.playerMP = theRPGPlayer.myStat.MP;
        saveData.playerMaxMP = theRPGPlayer.myStat.maxMp;
        saveData.playerEXP = theRPGPlayer.myStat.EXP;
        saveData.playerMaxEXP = theRPGPlayer.myStat.maxExp;
        saveData.playerLV = theRPGPlayer.Level;
        saveData.playerQuest = theRPGPlayer.quest;
        saveData.mySword = theRPGPlayer.mySword;
        if(QuestManager.Inst != null)
        {
            saveData.questNum = QuestManager.Inst.i;
        }
        else
        {
            saveData.questNum = theRPGPlayer.i;
        }

        saveData.Money = GameManager.Inst.Goldvalue;

        // �κ��丮 ���� ����
        InvenSlot[] slots = theInventoryManager.GetSlots();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                saveData.invenArrayNumber.Add(i);
                saveData.invenItemName.Add(slots[i].item.itemName);
                saveData.invenItemNumber.Add(slots[i].itemCount);
            }
        }

        // ��ų ���� ����
        List<SkillData> skillDatas = theSkillManager.GetSkills();
        for (int i = 0; i < skillDatas.Count; i++)
        {
            if (skillDatas[i] != null)
            {
                saveData.Datas.Add(skillDatas[i]);
            }
        }


        if (saveData.SceneName == "Village")
        {
            saveData.SceneNum = 1;
        }
        else if (saveData.SceneName == "Forest")
        {
            saveData.SceneNum = 2;
        }
        else if(saveData.SceneName == "Boss")
        {
            saveData.SceneNum = 3;
        }

        // ���� ��ü ����
        string json = JsonUtility.ToJson(saveData); // ���̽�ȭ

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);
    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            theRPGPlayer = ActionController.Inst.GetComponent<RPGPlayer>();
            theInventoryManager = InventoryManager.Inst;
            theSkillManager = SkillManager.Inst;

            // �÷��̾� ���� �ε�
            theRPGPlayer.transform.position = saveData.playerPos;
            theRPGPlayer.transform.eulerAngles = saveData.playerRot;
            theRPGPlayer.myStat.HP = saveData.playerHP;
            theRPGPlayer.myStat.maxHp = saveData.playerMaxHP;
            theRPGPlayer.myStat.MP = saveData.playerMP;
            theRPGPlayer.myStat.maxMp = saveData.playerMaxMP;
            theRPGPlayer.myStat.EXP = saveData.playerEXP;
            theRPGPlayer.myStat.maxExp = saveData.playerMaxEXP;
            theRPGPlayer.Level = saveData.playerLV;
            theRPGPlayer.quest = saveData.playerQuest;
            if (saveData.questNum > 0)
            {
                theRPGPlayer.i = saveData.questNum;
            }
            theRPGPlayer.mySword = saveData.mySword;

            GameManager.Inst.Goldvalue = saveData.Money;

            // �κ��丮 �ε�
            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                theInventoryManager.LoadToSlot(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenItemNumber[i]);
            }

            // ��ų �ε�
            for (int i = 0; i < saveData.Datas.Count; i++)
            {
                theSkillManager.LoadToSkill(saveData.Datas[i]);
            }
        }
    }
}
