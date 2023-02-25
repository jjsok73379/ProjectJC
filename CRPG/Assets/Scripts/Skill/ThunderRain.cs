using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderRain : NonTargetSkill
{
    protected override void OnHit()
    {
        Collider[] myEnemys = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y - disy, transform.position.z), 5f);
        foreach (Collider col in myEnemys)
        {
            col.GetComponent<Monster>()?.OnDamage(SkillDamage);
            col.GetComponent<Monster>()?.AddDebuff(Debuff.Type.Stun, 0.3f, 0.4f);
        }
    }
}
