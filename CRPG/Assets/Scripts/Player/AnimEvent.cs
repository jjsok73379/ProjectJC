using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public UnityEvent Skill = default;
    public UnityEvent Attack = default;
    public UnityEvent<bool> ComboCheck = default;
    public ParticleSystem BossBreath;

    public void FlameStart()
    {
        BossBreath.Play();
    }
    public void FlameEnd()
    {
        BossBreath.Stop();
    }

    public void OnAttack()
    {
        Attack?.Invoke();
    }

    public void OnSkill()
    {
        Skill?.Invoke();
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
