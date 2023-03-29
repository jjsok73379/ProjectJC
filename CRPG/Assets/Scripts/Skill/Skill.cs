using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Skill : MonoBehaviour
{
    [SerializeField]
    protected LayerMask enemyMask;
    public float SkillDamage;

    public void OnFire()
    {
        StartCoroutine(SkillTarget());
    }
    protected abstract IEnumerator SkillTarget();
    protected abstract void OnHit();
}
