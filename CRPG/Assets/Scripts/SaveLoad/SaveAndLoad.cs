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

    // 슬롯은 직령화가 불가능, 직렬화가 불가능한 애들이 있다.
    public List<int> invenArrayNumber = new List<int>();
    public List<string> invenItemName = new List<string>();
    public List<int> invenItemNumber = new List<int>();

    public List<SkillData> Datas = new List<SkillData>();
}

public class SaveAndLoad : MonoBehaviour
{
    public SaveData saveData = new SaveData();

    string SAVE_DATA_DIRECTORY; // 저장할 폴더 경로
    string SAVE_FILENAME = "/SaveFile.txt"; // 파일 이름

    RPGPlayer theRPGPlayer; // 플레이어의 정보 가져오기 위해 필요
    InventoryManager theInventoryManager; // 인벤토리 정보 가져오기 위해 필요
    SkillManager theSkillManager; // 스킬 정보 가져오기 위해 필요
    // Start is called before the first frame update
    void Start()
    {
        SAVE_DATA_DIRECTORY = Application.dataPath + $"/Save/";

        if (!Directory.Exists(SAVE_DATA_DIRECTORY)) // 해당 경로가 존재하지 않는다면
        {
            Directory.CreateDirectory(SAVE_DATA_DIRECTORY); // 폴더 생성
        }
    }

    public void SaveData()
    {
        theRPGPlayer = FindObjectOfType<RPGPlayer>();
        theInventoryManager = FindObjectOfType<InventoryManager>();
        theSkillManager = FindObjectOfType<SkillManager>();

        saveData.SceneName = SceneManager.GetActiveScene().name;

        // 플레이어 정보 저장
        saveData.playerPos = theRPGPlayer.transform.position;
        saveData.playerRot = theRPGPlayer.transform.rotation.eulerAngles;
        saveData.playerHP = theRPGPlayer.myStat.HP;
        saveData.playerMP = theRPGPlayer.myStat.MP;
        saveData.playerEXP = theRPGPlayer.myStat.EXP;
        saveData.playerLV = theRPGPlayer.Level;
        saveData.playerQuest = theRPGPlayer.quest;

        // 인벤토리 정보 저장
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

        // 스킬 정보 저장
        List<SkillData> skillDatas = theSkillManager.GetSkills();
        for (int i = 0; i < skillDatas.Count; i++)
        {
            if (skillDatas[i] != null)
            {
                saveData.Datas.Add(skillDatas[i]);
            }
        }

        // 최종 전체 저장
        string json = JsonUtility.ToJson(saveData); // 제이슨화

        File.WriteAllText(SAVE_DATA_DIRECTORY + SAVE_FILENAME, json);

    }

    public void LoadData()
    {
        if (File.Exists(SAVE_DATA_DIRECTORY + SAVE_FILENAME))
        {
            // 전체 읽어오기
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

            // 플레이어 정보 로드
            theRPGPlayer.transform.position = saveData.playerPos;
            theRPGPlayer.transform.eulerAngles = saveData.playerRot;
            theRPGPlayer.myStat.HP = saveData.playerHP;
            theRPGPlayer.myStat.MP = saveData.playerMP;
            theRPGPlayer.myStat.EXP = saveData.playerEXP;
            theRPGPlayer.Level = saveData.playerLV;
            theRPGPlayer.quest = saveData.playerQuest;

            // 인벤토리 로드
            for (int i = 0; i < saveData.invenItemName.Count; i++)
            {
                theInventoryManager.LoadToSlot(saveData.invenArrayNumber[i], saveData.invenItemName[i], saveData.invenArrayNumber[i]);
            }

            // 스킬 로드
            for (int i = 0; i < saveData.Datas.Count; i++)
            {
                theSkillManager.LoadToSkill(saveData.Datas[i]);
            }
        }
        else
        {
            Debug.Log("세이브 파일이 없습니다.");
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
