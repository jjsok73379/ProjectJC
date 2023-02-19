using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLaser : TargetingProjectile
{
    protected override void OnHit()
    {
        Instantiate(hitEffect, transform.position, Quaternion.identity);
    }
}
