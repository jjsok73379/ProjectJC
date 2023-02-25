using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public UnityEvent Attack = default;
    public UnityEvent<bool> ComboCheck = default;
    public Transform myTarget;
    public GameObject SkillEff;

    public void OnAttack()
    {
        Attack?.Invoke();
    }

    public void OnSkill()
    {
        Instantiate(SkillEff, myTarget.position, Quaternion.identity);
    }

    public void ComboCheckStart()
    {
        ComboCheck?.Invoke(true);
    }

    public void ComboCheckEnd()
    {
        ComboCheck?.Invoke(false);
    }
}
