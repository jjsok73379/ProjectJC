using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPost : MonoBehaviour
{
    [SerializeField]
    TMP_Text QuestName;
    public Toggle QuestChk;
    public TMP_Text QuestCount;
    [SerializeField]
    RPGPlayer theRPGPlayer;

    public void HaveQuest()
    {
        QuestName.gameObject.SetActive(true);
        QuestName.text = theRPGPlayer.quest.title;
        if (theRPGPlayer.quest.goal.goalType == GoalType.Etc)
        {
            QuestChk.gameObject.SetActive(true);
        }
        else if (theRPGPlayer.quest.goal.goalType != GoalType.Etc)
        {
            QuestCount.gameObject.SetActive(true);
            QuestCount.text = "( " + theRPGPlayer.quest.goal.currentAmount.ToString() + " / " + theRPGPlayer.quest.goal.requiredAmount.ToString() + " )";
        }
    }

    public void NoQuest()
    {
        QuestName.gameObject.SetActive(false);
        QuestChk.gameObject.SetActive(false);
        QuestCount.gameObject.SetActive(false);
    }
}
