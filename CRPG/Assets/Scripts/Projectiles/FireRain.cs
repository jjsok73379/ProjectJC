using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRain : NonTargetingProjectile
{
    protected override void OnHit()
    {
        Instantiate(hitEffect, transform.position, Quaternion.identity);

        Vector3 pos = new Vector3(0, 5, 0);
        Collider[] list = Physics.OverlapBox(transform.position, pos, Quaternion.identity, myEnemy);
        foreach (Collider col in list)
        {
            col.GetComponent<Monster>()?.OnDamage(SkillPoint);
        }
    }
}
