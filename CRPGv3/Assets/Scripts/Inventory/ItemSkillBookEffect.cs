using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffect/SkillBook")]
public class ItemSkillBookEffect : ItemEffect
{
    public int SkillBookPoint = 0;
    public override bool ExecuteRole()
    {
        return true;
    }
}
