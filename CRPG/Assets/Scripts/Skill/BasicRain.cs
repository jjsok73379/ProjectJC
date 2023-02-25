using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRain : NonTargetSkill
{
    protected override void OnHit()
    {
        Collider[] myEnemys = Physics.OverlapSphere(new Vector3(transform.position.x, transform.position.y - disy, transform.position.z), 8f);
        foreach(Collider col in myEnemys)
        {
            col.GetComponent<Monster>()?.OnDamage(SkillDamage);
        }
    }
}
