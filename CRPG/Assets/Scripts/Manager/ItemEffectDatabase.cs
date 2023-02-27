using CombineRPG;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[Serializable]
public class PotionItemEffect
{
    public string itemName;  // �������� �̸�(Key������ ����� ��)
    public int num;  // ��ġ
}

[Serializable]
public class SkillBookItemEffect
{
    public string itemName;  // �������� �̸�(Key������ ����� ��)
    public SkillData Eff;  // ȿ��
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
                        case "ü�� 30 ȸ�� ����":
                            thePlayer.IncreaseHP(PotionitemEffects[0].num);
                            break;
                        case "ü�� 50 ȸ�� ����":
                            thePlayer.IncreaseHP(PotionitemEffects[1].num);
                            break;
                        case "ü�� 100 ȸ�� ����":
                            thePlayer.IncreaseHP(PotionitemEffects[2].num);
                            break;
                        case "ü�� 300 ȸ�� ����":
                            thePlayer.IncreaseHP(PotionitemEffects[3].num);
                            break;
                        case "ü�� 500 ȸ�� ����":
                            thePlayer.IncreaseHP(PotionitemEffects[4].num);
                            break;
                        case "ü�� 1000 ȸ�� ����":
                            thePlayer.IncreaseHP(PotionitemEffects[5].num);
                            break;
                        case "ü�� 3000 ȸ�� ����":
                            thePlayer.IncreaseHP(PotionitemEffects[6].num);
                            break;
                        case "ü�� 5000 ȸ�� ����":
                            thePlayer.IncreaseHP(PotionitemEffects[7].num);
                            break;
                        case "���� 30 ȸ�� ����":
                            thePlayer.IncreaseMP(PotionitemEffects[8].num);
                            break;
                        case "���� 50 ȸ�� ����":
                            thePlayer.IncreaseMP(PotionitemEffects[9].num);
                            break;
                        case "���� 100 ȸ�� ����":
                            thePlayer.IncreaseMP(PotionitemEffects[10].num);
                            break;
                        case "���� 300 ȸ�� ����":
                            thePlayer.IncreaseMP(PotionitemEffects[11].num);
                            break;
                        case "���� 500 ȸ�� ����":
                            thePlayer.IncreaseMP(PotionitemEffects[12].num);
                            break;
                        case "���� 1000 ȸ�� ����":
                            thePlayer.IncreaseMP(PotionitemEffects[13].num);
                            break;
                        case "���� 3000 ȸ�� ����":
                            thePlayer.IncreaseMP(PotionitemEffects[14].num);
                            break;
                        case "���� 5000 ȸ�� ����":
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
                        case "��ų�� : �⺻ ��":
                            theSkillManager.mySkill = SkillBookitemEffects[0].Eff;
                            break;
                        case "��ų�� : �⺻ ����":
                            theSkillManager.mySkill = SkillBookitemEffects[1].Eff;
                            break;
                        case "��ų�� : �⺻ ��":
                            theSkillManager.mySkill = SkillBookitemEffects[2].Eff;
                            break;
                        case "��ų�� : ȭ��":
                            theSkillManager.mySkill = SkillBookitemEffects[3].Eff;
                            break;
                        case "��ų�� : ����":
                            theSkillManager.mySkill = SkillBookitemEffects[4].Eff;
                            break;
                        case "��ų�� : ����":
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
