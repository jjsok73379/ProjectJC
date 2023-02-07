using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CombinedSkill : MonoBehaviour
{
    public CombinedSkillData mySkillData;
    SkillManager skill;

    // Start is called before the first frame update
    void Start()
    {
        skill = SkillManager.Inst;
    }

    private void Update()
    {
        if (!skill.CombineOpen)
        {
            skill.mySkill = mySkillData;
            skill.AddSkill();
        }
    }

    public void AddToList()
    {
        skill.mySkill = mySkillData;
        skill.AddSkill();
    }
}
