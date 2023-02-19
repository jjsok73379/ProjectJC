using CombineRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum SkillType
{
    Targeting, NonTargeting, Material
}

[Serializable]
public struct SkillStat
{
    public SkillData orgData;
    public int Level;
    public SkillData Material
    {
        get => orgData.Materials[2];
    }
    public int MaterialCount
    {
        get => orgData.GetMaterialCount(Level);
    }
    public int UpgradeCount
    {
        get => orgData.GetMaterialCount(Level + 1);
    }
    public int TotalCount
    {
        get
        {
            int total = 0;
            for(int i = 1; i <= Level; i++)
            {
                total += orgData.GetMaterialCount(i);
            }
            return total;
        }
    }
    public bool IsUpgradable
    {
        get => Level < orgData.GetMaxLevel();
    }
    public float SkillDamage
    {
        get => orgData.SkillDamage(Level);
    }
    public float AttackRange
    {
        get => orgData.GetAttackRange();
    }
}

public class Skill : MonoBehaviour
{
    [SerializeField]
    protected SkillStat myStat;
    public int MaterialCount
    {
        get => myStat.MaterialCount;
    }
    public int UpgradeCount
    {
        get => myStat.UpgradeCount;
    }
    public int TotalCount
    {
        get => myStat.TotalCount;
    }
    public bool IsUpgradable
    {
        get => myStat.IsUpgradable;
    }
    public enum State
    {
        Create, Activate, UnActivate
    }
    public State myState = State.Create;

    public List<Transform> myEnemys = new List<Transform>();
    public Monster myTarget = null;
    public LayerMask enemyMask;
    public Transform mySkillPoint;
    public GameObject mySkill;
    public void UnActivate()
    {
        myState = State.UnActivate;
        SkillManager.Inst.theRPGPlayer.myStat.AttackRange = SkillManager.Inst.theRPGPlayer.orgRange;
        StopAllCoroutines();
    }
    public void OnActivate()
    {
        myState = State.Activate;
        SkillManager.Inst.theRPGPlayer.myStat.AttackRange = myStat.AttackRange;
    }

    public void OnUpgrade()
    {
        myStat.Level++;
    }
}
