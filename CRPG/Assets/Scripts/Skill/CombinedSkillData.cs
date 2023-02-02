using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CombinedSkillData", menuName = "ScriptableObjects/CombinedSkillData", order = 1)]
public class CombinedSkillData : SkillData
{
    public List<SkillData> Materials;
}
