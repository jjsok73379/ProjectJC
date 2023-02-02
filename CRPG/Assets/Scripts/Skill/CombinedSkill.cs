using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedSkill : MonoBehaviour
{
    public SkillData mySkillData;
    SkillManager skill;

    // Start is called before the first frame update
    void Start()
    {
        skill = SkillManager.Inst;
    }

    public void AddToList()
    {
        skill.mySkill = mySkillData;
        skill.AddSkill();
    }
}
