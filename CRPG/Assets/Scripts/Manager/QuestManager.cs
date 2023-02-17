using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : Singleton<QuestManager>
{
    public static bool QuestActivated = false;
    public Quest[] quest;

    public RPGPlayer theRPGPlayer;

    public GameObject QuestPrefab;
    [SerializeField]
    GameObject QuestUI;
    [SerializeField]
    GameObject QuestContent;
    [SerializeField]
    GameObject UIContent;
    public GameObject objQ;
    public GameObject objQ2;
    public int i = 0;

    private void Start()
    {
        QuestUI.SetActive(false);
        OpenQuestWindow();
    }

    private void Update()
    {
        TryOpenQuestUI();
    }

    void TryOpenQuestUI()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuestActivated = !QuestActivated;

            if (QuestActivated)
            {
                QuestUI.SetActive(true);
            }
            else
            {
                QuestUI.SetActive(false);
            }
        }
        if(!QuestActivated)
        {
            if (objQ2 != null)
            {
                objQ2.GetComponent<QuestInfo>().myInfo.SetActive(false);
            }
        }
    }

    public void OpenQuestWindow()
    {
        objQ = Instantiate(QuestPrefab, QuestContent.transform);
        for(int j=0;j< objQ.GetComponent<QuestInfo>().titleText.Length; j++)
        {
            objQ.GetComponent<QuestInfo>().titleText[j].text = quest[i].title;
        }
        objQ.GetComponent<QuestInfo>().descriptionText.text = quest[i].description;
        objQ.GetComponent<QuestInfo>().EXP_text.text = quest[i].experienceReward.ToString();
        objQ.GetComponent<QuestInfo>().goldText.text = quest[i].goldReward.ToString();
        objQ.GetComponent<QuestInfo>().BeforeAccept();
    }

    public void AcceptQuest()
    {
        objQ.GetComponent<QuestInfo>().myInfo.SetActive(false);
        quest[i].isActive = true;
        // RPGPlayer에게 주기
        theRPGPlayer.quest = quest[i];
        objQ2 = Instantiate(QuestPrefab, UIContent.transform);
        for (int j = 0; j < objQ2.GetComponent<QuestInfo>().titleText.Length; j++)
        {
            objQ2.GetComponent<QuestInfo>().titleText[j].text = quest[i].title;
        }
        objQ2.GetComponent<QuestInfo>().descriptionText.text = quest[i].description;
        objQ2.GetComponent<QuestInfo>().EXP_text.text = quest[i].experienceReward.ToString();
        objQ2.GetComponent<QuestInfo>().goldText.text = quest[i].goldReward.ToString();
        objQ2.GetComponent<QuestInfo>().AfterAccept();
        objQ.GetComponent<QuestInfo>().AfterAccept();
    }

    public void ForgiveQuest()
    {
        Destroy(objQ2);
        Destroy(objQ);
        OpenQuestWindow();
    }
}
