using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sword : MonoBehaviour
{
    public Sprite myImage;
    public Transform mySkillPoint;
    public string SwordName; // 검의 이름
    public float range; // 검의 사정 거리
    public float AttackSpeed; // 검의 공격속도

    public int damage; // 검의 공격력

    //public ParticleSystem SlashParticle; // 검의 타격 이펙트
    //public AudioClip Slash_Sound;
}
