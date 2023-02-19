using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public GameObject hitEffect;
    [SerializeField]
    protected LayerMask myEnemy;
    [field:SerializeField]
    public float moveSpeed
    {
        get;
        private set;
    }
    protected float SkillPoint
    {
        get;
        private set;
    }
    protected Monster myTarget = null;
    public void OnFire(Monster target, LayerMask mask, float damage)
    {
        SkillPoint = damage;
        myTarget = target;
        StartCoroutine(Attacking(mask));
    }
    protected abstract IEnumerator Attacking(LayerMask mask);
    protected abstract void OnHit();
}
