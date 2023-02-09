using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemEffect
{
    public string itemName; // 아이템의 이름(Key값으로 사용할 것)
    [Tooltip("HP, MP")]
    public string[] part; // 효과
    public int[] num; //수치
}

public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    ItemEffect[] itemEffects;

    const string HP = "HP", MP = "MP";

    [SerializeField]
    CharacterStat thePlayerStat;
    [SerializeField]
    WeaponManager theWeaponManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
