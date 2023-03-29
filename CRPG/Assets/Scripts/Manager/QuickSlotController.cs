using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotController : MonoBehaviour
{
    [SerializeField] private InvenSlot[] quickSlots;  // 퀵슬롯들 (5개)
    [SerializeField] private Transform tf_parent;  // 퀵슬롯들의 부모 오브젝트

    private int selectedSlot;  // 선택된 퀵슬롯의 인덱스 (0~5)

    [SerializeField]
    private WeaponManager theWeaponManager;
    [SerializeField]
    private ItemEffectDatabase theItemEffectDatabase;
    [SerializeField] 
    private Image[] img_CoolTime;  // 퀵슬롯 쿨타임 이미지들
    [SerializeField]
    ActionController theActionController;

    // 쿨타임 내용
    [SerializeField]
    private float coolTime;  // 정해짐 쿨타임  [SerializeField]로 유니티 인스펙터에서 결정
    private float currentCoolTime;  // coolTime 을 시작점으로 0 이 될 때까지 감소 업뎃
    private bool isCoolTime = false;  // 현재 쿨타임 중인지


    void Start()
    {
        quickSlots = tf_parent.GetComponentsInChildren<InvenSlot>();
        for(int i = 0; i < img_CoolTime.Length; i++)
        {
            img_CoolTime[i].fillAmount = 0;
        }
    }

    void Update()
    {
        TryInputNumber();
        CoolTimeCalc();
    }

    private void TryInputNumber()
    {
        if (!isCoolTime)
        {
            if (Input.GetKeyDown(KeyCode.F1))
                UseSlot(0);
            else if (Input.GetKeyDown(KeyCode.F2))
                UseSlot(1);
            else if (Input.GetKeyDown(KeyCode.F3))
                UseSlot(2);
            else if (Input.GetKeyDown(KeyCode.F4))
                UseSlot(3);
            else if (Input.GetKeyDown(KeyCode.F5))
                UseSlot(4);
        }
    }

    private void CoolTimeReset()
    {
        currentCoolTime = coolTime;
        isCoolTime = true;
    }

    private void CoolTimeCalc()
    {
        if (isCoolTime)
        {
            currentCoolTime -= Time.deltaTime;  // 1 초에 1 씩 감소

            for (int i = 0; i < img_CoolTime.Length; i++)
            {
                img_CoolTime[i].fillAmount = currentCoolTime / coolTime;
            }

            if (currentCoolTime <= 0)
            {
                isCoolTime = false;
            }
        }
    }

    private void UseSlot(int _num)
    {
        selectedSlot = _num;
        EatItem();
    }

    public void IsActivatedQuickSlot(int _num)
    {
        if (selectedSlot == _num)
        {
            EatItem();
            return;
        }
        if (DragSlot.Inst != null)
        {
            if (DragSlot.Inst.dragSlot != null)
            {
                if (DragSlot.Inst.dragSlot.GetQuickSlotNumber() == selectedSlot)
                {
                    EatItem();
                    return;
                }
            }
        }
    }

    public void EatItem()
    {
        CoolTimeReset();

        if (quickSlots[selectedSlot].item == null)
        {
            StartCoroutine(theActionController.WhenNoItem());
        }
        else
        {
            theItemEffectDatabase.UseItem(quickSlots[selectedSlot].item);
            quickSlots[selectedSlot].SetSlotCount(-1);
        }
    }

    public bool GetIsCoolTime()
    {
        return isCoolTime;
    }
}
