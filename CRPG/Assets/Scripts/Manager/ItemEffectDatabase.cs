using CombineRPG;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

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
    [SerializeField]
    SlotToolTip theSlotToolTip;
    [SerializeField]
    QuickSlotController theQuickSlotController;
    [SerializeField]
    InventoryManager theInventoryManager;
    [SerializeField]
    StoreToolTip theStoreToolTip;
    [SerializeField]
    CharacterInfo theCharacterInfo;

    private void Awake()
    {
        Inst = this;
    }

    public void SellItem(Item _item)
    {
        GameManager.Inst.Goldvalue += _item.SellPrice;
    }

    public void UseItem(Item _item)
    {
        if (_item.itemType == Item.ItemType.Equipment)
        {
            StartCoroutine(theWeaponManager.ChangeWeaponCoroutine(_item.itemName));
            theCharacterInfo.SwordImage.sprite = _item.itemImage;
            thePlayer.mySword = _item.itemPrefab.GetComponent<Sword>();
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
                        case "체력 30 회복 포션":
                            thePlayer.IncreaseHP(PotionitemEffects[0].num);
                            break;
                        case "체력 50 회복 포션":
                            thePlayer.IncreaseHP(PotionitemEffects[1].num);
                            break;
                        case "체력 100 회복 포션":
                            thePlayer.IncreaseHP(PotionitemEffects[2].num);
                            break;
                        case "체력 300 회복 포션":
                            thePlayer.IncreaseHP(PotionitemEffects[3].num);
                            break;
                        case "체력 500 회복 포션":
                            thePlayer.IncreaseHP(PotionitemEffects[4].num);
                            break;
                        case "체력 1000 회복 포션":
                            thePlayer.IncreaseHP(PotionitemEffects[5].num);
                            break;
                        case "체력 3000 회복 포션":
                            thePlayer.IncreaseHP(PotionitemEffects[6].num);
                            break;
                        case "체력 5000 회복 포션":
                            thePlayer.IncreaseHP(PotionitemEffects[7].num);
                            break;
                        case "마나 30 회복 포션":
                            thePlayer.IncreaseMP(PotionitemEffects[8].num);
                            break;
                        case "마나 50 회복 포션":
                            thePlayer.IncreaseMP(PotionitemEffects[9].num);
                            break;
                        case "마나 100 회복 포션":
                            thePlayer.IncreaseMP(PotionitemEffects[10].num);
                            break;
                        case "마나 300 회복 포션":
                            thePlayer.IncreaseMP(PotionitemEffects[11].num);
                            break;
                        case "마나 500 회복 포션":
                            thePlayer.IncreaseMP(PotionitemEffects[12].num);
                            break;
                        case "마나 1000 회복 포션":
                            thePlayer.IncreaseMP(PotionitemEffects[13].num);
                            break;
                        case "마나 3000 회복 포션":
                            thePlayer.IncreaseMP(PotionitemEffects[14].num);
                            break;
                        case "마나 5000 회복 포션":
                            thePlayer.IncreaseMP(PotionitemEffects[15].num);
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
                        case "스킬북 : 기본 구":
                            theSkillManager.mySkill = SkillBookitemEffects[0].Eff;
                            break;
                        case "스킬북 : 기본 폭발":
                            theSkillManager.mySkill = SkillBookitemEffects[1].Eff;
                            break;
                        case "스킬북 : 기본 비":
                            theSkillManager.mySkill = SkillBookitemEffects[2].Eff;
                            break;
                        case "스킬북 : 화염":
                            theSkillManager.mySkill = SkillBookitemEffects[3].Eff;
                            break;
                        case "스킬북 : 얼음":
                            theSkillManager.mySkill = SkillBookitemEffects[4].Eff;
                            break;
                        case "스킬북 : 번개":
                            theSkillManager.mySkill = SkillBookitemEffects[5].Eff;
                            break;
                    }
                    if (theSkillManager.mySkills.Contains(theSkillManager.mySkill))
                    {
                        return;
                    }
                    else
                    {
                        theSkillManager.AddSkill();
                    }
                }
            }
        }
        if (thePlayer.quest.isActive)
        {
            thePlayer.quest.goal.IsDoAction();
        }
    }

    public void ShowToolTip(Item _item, Vector3 _pos, int Pricenum)
    {
        theSlotToolTip.ShowToolTip(_item, _pos, Pricenum);
    }

    public void HideToolTip()
    {
        theSlotToolTip.HideToolTip();
    }
    public void ShowStoreToolTip(Item _item, Vector3 _pos, int Pricenum)
    {
        theStoreToolTip.ShowToolTip(_item, _pos, Pricenum);
    }
    public void HideStoreToolTip()
    {
        theStoreToolTip.HideToolTip();
    }

    public void IsActivatedquickSlot(int _num)
    {
        theQuickSlotController.IsActivatedQuickSlot(_num);
    }

    public bool GetIsCoolTime()
    {
        return theQuickSlotController.GetIsCoolTime();
    }

    public bool GetIsFull()
    {
        return theInventoryManager.GetIsFull();
    }

    public void SetIsFull(bool _flag)
    {
        theInventoryManager.SetIsFull(_flag);
    }
}
