using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderLaser : TargetingProjectile
{
    protected override void OnHit()
    {
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Collider[] list = Physics.OverlapSphere(transform.position, 0.5f, myEnemy);
        foreach (Collider col in list)
        {
            col.GetComponent<Monster>().AddDebuff(Debuff.Type.Stun, 0.5f, 0.5f);
        }
    }
}
