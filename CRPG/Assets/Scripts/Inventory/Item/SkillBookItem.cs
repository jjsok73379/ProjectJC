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
            // �ӽ� : ���� �ϳ� ����
            Amount--;

            return true;
        }

        protected override CountableItem Clone(int amount)
        {
            return new SkillBookItem(CountableData as SkillBookItemData, amount);
        }
    }
}
