using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemEffect
{
    public string itemName; // �������� �̸�(Key������ ����� ��)
    [Tooltip("HP, MP")]
    public string[] part; // ȿ��
    public int[] num; //��ġ
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
