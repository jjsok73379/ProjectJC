using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public string effectName;
    public abstract bool ExecuteRole();
}
