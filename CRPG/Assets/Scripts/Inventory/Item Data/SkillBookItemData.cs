using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.InventorySystem
{
    /// <summary> �Һ� ������ ���� </summary>
    [CreateAssetMenu(fileName = "Item_SkillBook_", menuName = "Inventory System/Item Data/SkillBook", order = 3)]
    public class SkillBookItemData : CountableItemData
    {
        /// <summary> ȿ����(ȸ���� ��) </summary>
        public float Value => _value;
        [SerializeField] private float _value;
        public override Item CreateItem()
        {
            return new SkillBookItem(this);
        }
    }
}
