using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    [SerializeField]
    GameObject myPlayer;
    public bool IsEnter = false;
    public void EnterBossZone()
    {
        myPlayer.transform.position = new Vector3(52.0f, 2.5f, 59.0f);
        IsEnter = true;
        gameObject.SetActive(false);
    }

    public void CloseBossText()
    {
        ActionController.Inst.BossText.SetActive(false);
    }
}
