using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment, SkillBook, Potion
}

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 1)]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite itemImage;
    public ItemEffect itemEffects;

    public bool Use()
    {
        bool isUsed = false;
        isUsed = true;
        return isUsed;
    }
}
