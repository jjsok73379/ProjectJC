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
        if (goalType == GoalType.Kill)
        {
            if (monster == Target)
            {
                currentAmount++;
            }
        }
    }

    public void ItemCollected(GameObject item)
    {
        if (goalType == GoalType.Gathering)
        {
            if(item == Target)
            {
                currentAmount++;
            }
        }
    }

    public void IsDoAction()
    {
        if (goalType == GoalType.Etc)
        {
            donechk = true;
        }
    }
}

public enum GoalType
{
    Kill,
    Gathering,
    Etc
}
