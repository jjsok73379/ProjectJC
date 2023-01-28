using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Targeting, nonTargeting, Follow, Rotate, Material
}

[CreateAssetMenu(fileName = "SkillData", menuName = "ScriptableObjects/SkillDate", order = 1)]
public class SkillData : ScriptableObject
{
    public Sprite myImage;
    [SerializeField] string myInfo;
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
