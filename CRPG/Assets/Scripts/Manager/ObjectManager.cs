using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public GameObject CoinPrefab;
    public int initialItems = 10;
    public List<GameObject> coins = new List<GameObject>();

    private void Awake()
    {
        MakeCoins();
    }

    void MakeCoins()
    {
        for(int i=0;i< initialItems; i++)
        {
            GameObject tempItem = Instantiate(CoinPrefab);
            tempItem.transform.parent = transform;
            tempItem.SetActive(false);
            coins.Add(tempItem);
        }
    }

    public void DropCoinToPosition(Vector3 pos, int coinValue)
    {
        GameObject reusedItem = null;
        for (int i = 0; i < coins.Count; i++)
        {
            if (coins[i].activeSelf == false)
            {
                reusedItem = coins[i];
                break;
            }
        }
        if (reusedItem == null)
        {
            GameObject newItem = Instantiate(CoinPrefab);
            coins.Add(newItem);
            reusedItem = newItem;
        }
        reusedItem.SetActive(true);
        reusedItem.GetComponent<Coin>().SetCoinValue(coinValue);
        reusedItem.transform.position = new Vector3(pos.x, reusedItem.transform.position.y, pos.z);
    }
}
