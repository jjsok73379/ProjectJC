using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPost : MonoBehaviour
{
    public static QuestPost Inst;

    [SerializeField]
    TMP_Text QuestName;
    public Toggle QuestChk;
    public TMP_Text QuestCount;

    private void Awake()
    {
        Inst = this;
    }

    private void Update()
    {
        if(ActionController.Inst != null)
        {
            if (ActionController.Inst.GetComponent<RPGPlayer>().quest != null)
            {
                if (DataManager.Inst.IsQuesting)
                {
                    HaveQuest();
                }
            }
        }
    }

    public void HaveQuest()
    {
        QuestName.text = ActionController.Inst.GetComponent<RPGPlayer>().quest.title;
        if (ActionController.Inst.GetComponent<RPGPlayer>().quest.goal.goalType == GoalType.Etc)
        {
            QuestChk.gameObject.SetActive(true);
            QuestCount.gameObject.SetActive(false);
        }
        else if (ActionController.Inst.GetComponent<RPGPlayer>().quest.goal.goalType != GoalType.Etc)
        {
            QuestCount.gameObject.SetActive(true);
            QuestChk.gameObject.SetActive(false);
            QuestCount.text = "( " + ActionController.Inst.GetComponent<RPGPlayer>().quest.goal.currentAmount.ToString() + " / " + ActionController.Inst.GetComponent<RPGPlayer>().quest.goal.requiredAmount.ToString() + " )";
        }
    }
}
