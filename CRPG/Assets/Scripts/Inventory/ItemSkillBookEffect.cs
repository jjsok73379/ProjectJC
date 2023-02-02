using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/SkillBook")]
public class ItemSkillBookEffect : ItemEffect
{
    public override bool ExecuteRole()
    {
        return true;
    }
}
