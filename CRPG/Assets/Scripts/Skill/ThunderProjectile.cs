using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderProjectile : Projectile
{
    protected override void OnHit()
    {
        SoundManager.Inst.ThunderSkillSound.Play();
        Instantiate(myEff, transform.position, Quaternion.identity);
        myTarget.AddDebuff(Debuff.Type.Stun, 0.3f, 0.7f);
    }
}
