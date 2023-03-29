using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField]
    Sword currentSword; // 현재 들고 있는 검

    private void Start()
    {
        if (currentSword != null)
        {
            WeaponManager.currentWeapon = currentSword.GetComponent<Transform>();
        }
    }

    public void SwordChange(Sword _sword)
    {
        if(WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }

        currentSword = _sword;
        WeaponManager.currentWeapon = currentSword.GetComponent<Transform>();

        currentSword.transform.localPosition = Vector3.zero;
        currentSword.gameObject.SetActive(true);
    }
}
