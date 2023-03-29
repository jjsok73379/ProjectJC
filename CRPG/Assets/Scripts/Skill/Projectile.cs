using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Skill
{
    public Monster myTarget;
    [SerializeField]
    protected GameObject myEff;
    protected override IEnumerator SkillTarget()
    {
        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.zero;
        float radius = transform.localScale.x * 0.5f;
        while (Vector3.Distance(pos, transform.position) > Mathf.Epsilon)
        {
            float delta = Time.fixedDeltaTime * 7.0f;
            if(myTarget != null)
            {
                pos = myTarget.AttackPoint.position;
            }
            dir = pos - transform.position;
            if (delta > dir.magnitude)
            {
                delta = dir.magnitude;
            }
            dir.Normalize();
            if (myTarget != null)
            {
                Ray ray = new Ray(transform.position, dir);
                if(Physics.Raycast(ray, out RaycastHit hit, delta + radius, enemyMask))
                {
                    transform.position = hit.point + -dir * radius;
                    Destroy(gameObject);
                    myTarget.OnDamage(SkillDamage);
                    OnHit();
                    yield break;
                }
            }
            transform.Translate(dir * delta, Space.World);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
