using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetingProjectile : Projectile
{
    protected override IEnumerator Attacking(LayerMask mask)
    {
        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.zero;
        float radius = transform.localScale.x * 0.5f;
        while (Vector3.Distance(pos, transform.position) > Mathf.Epsilon)
        {
            float delta = Time.deltaTime * moveSpeed;
            if (myTarget != null)
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
                if (Physics.Raycast(ray, out RaycastHit hit, delta + radius, mask))
                {
                    // 몬스터에 맞았을 때
                    transform.position = hit.point + -dir * radius;
                    Destroy(gameObject);
                    myTarget.OnDamage(SkillPoint);
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
