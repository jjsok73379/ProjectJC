using Rito.InventorySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rito.InventorySystem
{
    public class SkillBookItem : CountableItem, IUsableItem
    {
        public SkillBookItem(SkillBookItemData data, int amount = 1) : base(data, amount) { }

        public bool Use()
        {
            // 임시 : 개수 하나 감소
            Amount--;

            return true;
        }

        protected override CountableItem Clone(int amount)
        {
            return new SkillBookItem(CountableData as SkillBookItemData, amount);
        }
    }
}
