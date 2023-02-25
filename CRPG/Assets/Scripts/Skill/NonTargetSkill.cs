using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonTargetSkill : Skill
{
    [SerializeField]
    protected float disy;
    protected override IEnumerator SkillTarget()
    {
        OnHit();
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
}
