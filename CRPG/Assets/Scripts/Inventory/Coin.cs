using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [System.NonSerialized]
    public int money;

    public void SetCoinValue(int money)
    {
        this.money = money;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.Inst.AddGold(money);

            RemoveFromWorld();
        }
    }

    public void RemoveFromWorld()
    {
        gameObject.SetActive(false);
    }
}
