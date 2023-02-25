using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : Projectile
{
    protected override void OnHit()
    {
        myTarget.AddDebuff(Debuff.Type.Slow, 0.5f, 1.0f);
    }
}
