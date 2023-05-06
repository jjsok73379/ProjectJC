using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public Item item;
    [SerializeField]
    bool IsEquipItem;

    private void Start()
    {
        if (!IsEquipItem)
        {
            Destroy(gameObject, 10.0f);
        }
        if (InputNumber.Inst.IsDrop)
        {
            item = InputNumber.Inst.DropItemData;
            item.IsDropitem = true;
        }
        else
        {
            item.IsDropitem = false;
            item = GetComponent<PickupRandomItem>().RandomItem[Random.Range(0, GetComponent<PickupRandomItem>().RandomItem.Length)];
        }
    }
}
