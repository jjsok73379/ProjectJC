using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{
    protected override void OnHit()
    {
        Instantiate(myEff, transform.position, Quaternion.identity);
    }
}
