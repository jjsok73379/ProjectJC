using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillData", order = 1)]
public class SkillData : ScriptableObject
{
    public enum SkillType
    {
        Targeting, nonTargeting, Material
    }
    public string SkillName;
    public Sprite myImage = null;
    public string myInfo;
    public SkillType mySkillType;
    [SerializeField] float AttackRange;
    public float CoolTime;
    public float SkillDamage;
    public GameObject mySkill;

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
