using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Inst;
    public Quest[] quest;

    public RPGPlayer theRPGPlayer;

    public GameObject QuestPrefab;
    [SerializeField]
    GameObject QuestContent;
    [SerializeField]
    GameObject UIContent;
    public GameObject QuestionMark;
    public GameObject UnfinishQuestionMark;
    public GameObject ExclamationMark;
    public GameObject objQ;
    public GameObject objQ2;
    public int i = 0;

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        if (DataManager.Inst.IsFinishQuest)
        {
            QuestionMark.SetActive(true);
            UnfinishQuestionMark.SetActive(false);
            ExclamationMark.SetActive(false);
            DataManager.Inst.IsFinishQuest = false;
        }
        i = theRPGPlayer.i;
        OpenQuestWindow();
    }

    public void OpenQuestWindow()
    {
        if (i > 5)
        {
            QuestionMark.SetActive(false);
            UnfinishQuestionMark.SetActive(false);
            ExclamationMark.SetActive(false);
        }
        else
        {
            objQ = Instantiate(QuestPrefab, QuestContent.transform);
            for (int j = 0; j < objQ.GetComponent<QuestInfo>().titleText.Length; j++)
            {
                objQ.GetComponent<QuestInfo>().titleText[j].text = quest[i].title;
            }
            objQ.GetComponent<QuestInfo>().descriptionText.text = quest[i].description;
            objQ.GetComponent<QuestInfo>().EXP_text.text = quest[i].experienceReward.ToString();
            objQ.GetComponent<QuestInfo>().goldText.text = quest[i].goldReward.ToString();
            if (!DataManager.Inst.IsQuesting)
            {
                objQ.GetComponent<QuestInfo>().BeforeAccept();
            }
            else
            {
                objQ.GetComponent<QuestInfo>().AfterAccept();
            }
        }
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
        DontDestroyUI.Inst.QuestPost.SetActive(true);
    }

    public void ForgiveQuest()
    {
        Destroy(objQ2);
        Destroy(objQ);
        OpenQuestWindow();
    }
}
