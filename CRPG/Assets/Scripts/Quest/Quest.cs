using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class Quest
{
    public bool isActive;

    public string title;
    public string description;
    public int goldReward;
    public int experienceReward;

    public QuestGoal goal;

    public void Complete()
    {
        isActive = false;
        QuestManager.Inst.theQuestPost.QuestChk.isOn = false;
        QuestManager.Inst.i++;
        QuestManager.Inst.OpenQuestWindow();
    }
}
