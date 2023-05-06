using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public enum SkillType
    {
        Ball, Bomb, NonTargeting, Material
    }

    public string SkillName;
    public Sprite myImage = null;
    public int Level;
    public string myInfo;
    [SerializeField] 
    float AttackRange;
    [SerializeField]
    int _mana;
    public float CoolTime;
    [SerializeField]
    float[] skillDamage;
    [SerializeField] int[] MaterialCounts;
    public SkillData[] Materials;
    public GameObject mySkill;
    public SkillType myType;

    public int Mana
    {
        get => _mana;
        set => _mana = value;
    }

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
