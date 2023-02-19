using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonTargetingProjectile : Projectile
{
    protected override IEnumerator Attacking(LayerMask mask)
    {
        Vector3 dir = myTarget.transform.position - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        Vector3 pos = Input.mousePosition;
        while (dist > Mathf.Epsilon)
        {
            float delta = Time.deltaTime * moveSpeed;
            if (delta > dist) delta = dist;
            dist -= delta;

            transform.position = pos + Vector3.up * 3.0f;
            yield return null;
        }
        Destroy(gameObject);
        OnHit();
    }
}
