using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public struct CharacterStat
{
    [SerializeField] float hp;
    [SerializeField] float maxHp;
    [SerializeField] float mp;
    [SerializeField] float maxMp;
    [SerializeField] float exp;
    [SerializeField] float maxExp;
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float ap;
    [SerializeField] float attackDelay;
    [SerializeField] float attackRange;

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
            exp = Mathf.Clamp(value, 0.0f, maxExp);
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
    }
    public float AttackDelay
    {
        get => attackDelay;
    }
    public float AttackRange
    {
        get => attackRange;
    }
}
