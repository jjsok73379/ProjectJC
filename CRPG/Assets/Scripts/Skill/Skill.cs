using CombineRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


public class Skill : MonoBehaviour
{
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
        StopAllCoroutines();
    }
    public void OnActivate()
    {
        myState = State.Activate;
    }
}
