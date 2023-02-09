using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Inst;
    private void Awake()
    {
        Inst = this;
    }
    public static bool isChangeWeapon = false; // ���� ��ü �ߺ� ���� ����. (True�� ���ϰ�)

    [SerializeField]
    float changeweaponDelayTime; // ���� ��ü ������ �ð�. ���� ������ ���� �� ���� �ִ� �� �ð�. �뷫 Weapon_Out �ִϸ��̼� �ð�.
    [SerializeField]
    float changeweaponEndDelayTime; // ���� ��ü�� ������ ���� ����

    [SerializeField]
    Sword[] swords; // ��� ������ Į

    // ���� �������� �̸����� ���� ���� ������ �����ϵ��� Dictionary �ڷ� ���� ���.
    Dictionary<string, Sword> swordDictionary = new Dictionary<string, Sword>();

    public static Transform currentWeapon; // ���� ����

    [SerializeField]
    SwordController theSwordController;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < swords.Length; i++)
        {
            swordDictionary.Add(swords[i].SwordName, swords[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ChangeWeaponCoroutine(string _name)
    {
        isChangeWeapon = true;

        yield return new WaitForSeconds(changeweaponDelayTime);

        WeaponChange(_name);

        yield return new WaitForSeconds(changeweaponEndDelayTime);

        isChangeWeapon = false;
    }

    void WeaponChange(string _name)
    {
        theSwordController.SwordChange(swordDictionary[_name]);
    }
}
