using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : Projectile
{
    protected override void OnHit()
    {
        SoundManager.Inst.BasicSkillSound.Play();
        Instantiate(myEff, transform.position, Quaternion.identity);
    }
}
