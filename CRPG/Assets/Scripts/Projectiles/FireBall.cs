using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : TargetingProjectile
{
    protected override void OnHit()
    {
        Instantiate(hitEffect, transform.position, Quaternion.identity);
    }
}
