using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : Projectile
{
    protected override void OnHit()
    {
        Instantiate(myEff, transform.position, Quaternion.identity);
        myTarget.AddDebuff(Debuff.Type.Slow, 0.5f, 2.0f);
    }
}
