using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using CombineRPG;
using UnityEngine.SceneManagement;

[Serializable]
public class SaveData
{
    public string SceneName;
    public int SceneNum;

    public Vector3 playerPos;
    public Vector3 playerRot;
    public float playerHP;
    public float playerMP;
    public float playerEXP;
    public int playerLV;
    public Quest playerQuest;

    // ������ ����ȭ�� �Ұ���, ����ȭ�� �Ұ����� �ֵ��� �ִ�.
    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

    public List<SkillData> Datas = new List<SkillData>();
}

public class SaveAndLoad : MonoBehaviour
{
    public SaveData saveData = new SaveData();

    string SAVE_DATA_DIRECTORY; // ������ ���� ���
    string SAVE_FILENAME = "/SaveFile.txt"; // ���� �̸�

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
    }

    public void SaveData()
    {
        theRPGPlayer = FindObjectOfType<RPGPlayer>();
        theInventoryManager = FindObjectOfType<InventoryManager>();
        theSkillManager = FindObjectOfType<SkillManager>();

        saveData.SceneName = SceneManager.GetActiveScene().name;

        // �÷��̾� ���� ����
        saveData.playerPos = theRPGPlayer.transform.position;
        saveData.playerRot = theRPGPlayer.transform.rotation.eulerAngles;
        saveData.playerHP = theRPGPlayer.myStat.HP;
        saveData.playerMP = theRPGPlayer.myStat.MP;
        saveData.playerEXP = theRPGPlayer.myStat.EXP;
        saveData.playerLV = theRPGPlayer.Level;
        saveData.playerQuest = theRPGPlayer.quest;

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

        // ���� ��ü ����
        string json = JsonUtility.ToJson(saveData); // ���̽�ȭ

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            // ��ü �о����
            string loadJson = File.ReadAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME);
            saveData = JsonUtility.FromJson<SaveData>(loadJson);

            theRPGPlayer = FindObjectOfType<RPGPlayer>();
            theInventoryManager = FindObjectOfType<InventoryManager>();
            theSkillManager = FindObjectOfType<SkillManager>();

            if (saveData.SceneName == "Title")
            {
                saveData.SceneNum = 0;
            }
            else if (saveData.SceneName == "Village")
            {
                saveData.SceneNum = 1;
            }
            else if (saveData.SceneName == "Forest")
            {
                saveData.SceneNum = 2;
            }

            // �÷��̾� ���� �ε�
            theRPGPlayer.transform.position = saveData.playerPos;
            theRPGPlayer.transform.eulerAngles = saveData.playerRot;
            theRPGPlayer.myStat.HP = saveData.playerHP;
            theRPGPlayer.myStat.MP = saveData.playerMP;
            theRPGPlayer.myStat.EXP = saveData.playerEXP;
            theRPGPlayer.Level = saveData.playerLV;
            theRPGPlayer.quest = saveData.playerQuest;

            // �κ��丮 �ε�
            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                theInventoryManager.LoadToSlot(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenArrayNumber[i]);
            }

            // ��ų �ε�
            for (int i = 0; i < saveData.Datas.Count; i++)
            {
                theSkillManager.LoadToSkill(saveData.Datas[i]);
            }
        }
        else
        {
            Debug.Log("���̺� ������ �����ϴ�.");
        }
    }

    public void GoTitle()
    {
        Time.timeScale = 1;
        LoadManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
