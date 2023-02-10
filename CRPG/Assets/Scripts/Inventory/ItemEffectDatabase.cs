using CombineRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

[Serializable]
public class PotionItemEffect
{
    public string itemName;  // 아이템의 이름(Key값으로 사용할 것)
    public int num;  // 수치
}

[Serializable]
public class SkillBookItemEffect
{
    public string itemName;  // 아이템의 이름(Key값으로 사용할 것)
    public SkillData Eff;  // 효과
}

public class ItemEffectDatabase : MonoBehaviour
{
    public static ItemEffectDatabase Inst = null;

    [SerializeField]
    PotionItemEffect[] PotionitemEffects;
    [SerializeField]
    SkillBookItemEffect[] SkillBookitemEffects;

    [SerializeField]
    RPGPlayer thePlayer;
    [SerializeField]
    WeaponManager theWeaponManager;
    [SerializeField]
    SkillManager theSkillManager;
    private void Awake()
    {
        Inst = this;
    }

    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.itemName));
            thePlayer.WeaponStat();
        }
        if (_item.itemType == Item.ItemType.Potion)
        {
            for(int i = 0; i < PotionitemEffects.Length; i++)
            {
                if (PotionitemEffects[i].itemName == _item.itemName)
                {
                    switch (PotionitemEffects[i].itemName)
                    {
                        case "30Potion":
                            thePlayer.IncreaseHPMP(PotionitemEffects[0].num);
                            break;
                        case "50Potion":
                            thePlayer.IncreaseHPMP(PotionitemEffects[1].num);
                            break;
                        case "100Potion":
                            thePlayer.IncreaseHPMP(PotionitemEffects[2].num);
                            break;
                        case "300Potion":
                            thePlayer.IncreaseHPMP(PotionitemEffects[3].num);
                            break;
                        case "500Potion":
                            thePlayer.IncreaseHPMP(PotionitemEffects[4].num);
                            break;
                        case "1000Potion":
                            thePlayer.IncreaseHPMP(PotionitemEffects[5].num);
                            break;
                        case "3000Potion":
                            thePlayer.IncreaseHPMP(PotionitemEffects[6].num);
                            break;
                        case "5000Potion":
                            thePlayer.IncreaseHPMP(PotionitemEffects[7].num);
                            break;
                    }
                }
            }
        }
        if (_item.itemType == Item.ItemType.SkillBook)
        {
            for(int i = 0; i < SkillBookitemEffects.Length; i++)
            {
                if (SkillBookitemEffects[i].itemName== _item.itemName)
                {
                    switch (SkillBookitemEffects[i].itemName)
                    {
                        case "SkillBook : BasicBall":
                            theSkillManager.mySkill = SkillBookitemEffects[0].Eff;
                            theSkillManager.AddSkill();
                            break;
                        case "SkillBook : BasicLaser":
                            theSkillManager.mySkill = SkillBookitemEffects[1].Eff;
                            theSkillManager.AddSkill();
                            break;
                        case "SkillBook : BasicRain":
                            theSkillManager.mySkill = SkillBookitemEffects[2].Eff;
                            theSkillManager.AddSkill();
                            break;
                        case "SkillBook : Fire":
                            theSkillManager.mySkill = SkillBookitemEffects[3].Eff;
                            theSkillManager.AddSkill();
                            break;
                        case "SkillBook : Ice":
                            theSkillManager.mySkill = SkillBookitemEffects[4].Eff;
                            theSkillManager.AddSkill();
                            break;
                        case "SkillBook : Thunder":
                            theSkillManager.mySkill = SkillBookitemEffects[5].Eff;
                            theSkillManager.AddSkill();
                            break;
                    }
                }
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
