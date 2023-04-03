using CombineRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class QuestGoal
{
    public GoalType goalType;

    public int requiredAmount;
    public int currentAmount;
    public GameObject Target;

    public bool donechk = false;

    public bool IsReached()
    {
        return (currentAmount >= requiredAmount);
    }

    public bool IsDone()
    {
        return donechk;
    }

    public void EnemyKilled(GameObject monster)
    {
        if (monster.name == Target.name + "(Clone)" || monster.name == Target.name)
        {
            currentAmount++;
            QuestPost.Inst.QuestCount.text = "( " + ActionController.Inst.GetComponent<RPGPlayer>().quest.goal.currentAmount.ToString() + " / " + ActionController.Inst.GetComponent<RPGPlayer>().quest.goal.requiredAmount.ToString() + " )";
        }
    }

    public void ItemCollected(GameObject item)
    {
        if (item == Target)
        {
            currentAmount++;
            QuestPost.Inst.QuestCount.text = "( " + ActionController.Inst.GetComponent<RPGPlayer>().quest.goal.currentAmount.ToString() + " / " + ActionController.Inst.GetComponent<RPGPlayer>().quest.goal.requiredAmount.ToString() + " )";
        }
    }

    public void IsDoAction()
    {
        QuestPost.Inst.QuestChk.isOn = true;
        donechk = true;
    }
}

public enum GoalType
{
    Kill,
    Gathering,
    Etc
}
