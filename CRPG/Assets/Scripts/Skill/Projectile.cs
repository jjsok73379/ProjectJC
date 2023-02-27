using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Skill
{
    public Monster myTarget;
    protected override IEnumerator SkillTarget()
    {
        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.zero;
        float radius = transform.localScale.x;
        while (Vector3.Distance(pos, transform.position) > 0)
        {
            float delta = Time.deltaTime;
            if(myTarget != null)
            {
                pos = myTarget.AttackPoint.position;
            }
            dir = (pos - transform.position).normalized;
            if (delta > dir.magnitude)
            {
                delta = dir.magnitude;
            }
            if (myTarget != null)
            {
                Ray ray = new Ray(transform.position, dir);
                if (Physics.Raycast(ray, out RaycastHit hit, delta + radius, enemyMask))
                {
                    transform.position = hit.point + -dir * radius;
                    myTarget.OnDamage(SkillDamage);
                    Destroy(gameObject);
                    OnHit();
                    yield break;
                }
            }
            transform.Translate(dir * delta);
            yield return new WaitForSeconds(3.0f);
        }
        Destroy(gameObject);
    }
}
