using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public struct CharacterStat
{
    [SerializeField] 
    float hp;
    public float maxHp;
    [SerializeField] 
    float mp;
    public float maxMp;
    [SerializeField] 
    float exp;
    public float maxExp;
    [SerializeField] 
    float moveSpeed;
    [SerializeField] 
    float rotSpeed;
    [SerializeField]
    float ap;
    [SerializeField] 
    float attackDelay;
    [SerializeField] 
    float attackRange;

    public UnityAction<float> changeHp;
    public UnityAction<float> changeMp;
    public UnityAction<float> changeExp;
    public float HP 
    { 
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0.0f, maxHp);
            changeHp?.Invoke(hp / maxHp);
        }
    }
    public float MP
    {
        get => mp;
        set
        {
            mp = Mathf.Clamp(value, 0.0f, maxMp);
            changeMp?.Invoke(mp / maxMp);
        }
    }

    public float EXP
    {
        get => exp;
        set
        {
            exp = value;
            changeExp?.Invoke(exp / maxExp);
        }
    }

    public float MoveSpeed
    {
        get => moveSpeed;
    }
    public float RotSpeed
    {
        get => rotSpeed;
    }
    public float AP
    {
        get => ap;
        set => ap = value;
    }
    public float AttackDelay
    {
        get => attackDelay;
        set => attackDelay = value;
    }
    public float AttackRange
    {
        get => attackRange;
        set => attackRange = value;
    }
}
