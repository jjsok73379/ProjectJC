using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public string SkillName;
    public Sprite myImage = null;
    public string myInfo;
    [SerializeField] 
    float AttackRange;
    public float CoolTime;
    [SerializeField]
    float[] skillDamage;
    [SerializeField] int[] MaterialCounts;
    public List<SkillData> Materials;

    public int GetMaterialCount(int lv)
    {
        return MaterialCounts[lv - 1];
    }

    public int GetMaxLevel()
    {
        return MaterialCounts.Length;
    }

    public float SkillDamage(int lv)
    {
        return skillDamage[lv - 1];
    }

    public string MyInfo
    {
        get => myInfo;
        set => myInfo = value;
    }

    public float GetAttackRange()
    {
        return AttackRange;
    }
}
