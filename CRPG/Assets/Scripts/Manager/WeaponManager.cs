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
    public static bool isChangeWeapon = false; // 무기 교체 중복 실행 방지. (True면 못하게)

    [SerializeField]
    float changeweaponDelayTime; // 무기 교체 딜레이 시간. 총을 꺼내기 위해 손 집어 넣는 그 시간. 대략 Weapon_Out 애니메이션 시간.
    [SerializeField]
    float changeweaponEndDelayTime; // 무기 교체가 완전히 끝난 시점

    [SerializeField]
    Sword[] swords; // 모든 종류의 칼

    // 관리 차원에서 이름으로 쉽게 무기 접근이 가능하도록 Dictionary 자료 구조 사용.
    Dictionary<string, Sword> swordDictionary = new Dictionary<string, Sword>();

    public static Transform currentWeapon; // 현재 무기

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
