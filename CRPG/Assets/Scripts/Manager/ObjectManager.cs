using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public GameObject BookPrefab;
    public GameObject PotionPrefab;
    public GameObject WeaponPrefab;
    public GameObject CoinPrefab;
    public int initialItems = 10;
    public List<GameObject> books = new List<GameObject>();
    public List<GameObject> potions = new List<GameObject>();
    public List<GameObject> Weapons = new List<GameObject>();
    public List<GameObject> Coins = new List<GameObject>();

    private void Awake()
    {
        MakeItems(BookPrefab, books);
        MakeItems(PotionPrefab, potions);
        MakeItems(WeaponPrefab, Weapons);
        MakeItems(CoinPrefab, Coins);
    }

    void MakeItems(GameObject ItemPrefab, List<GameObject> Items)
    {
        for(int i=0;i< initialItems; i++)
        {
            GameObject tempItem = Instantiate(ItemPrefab);
            tempItem.transform.parent = transform;
            tempItem.SetActive(false);
            Items.Add(tempItem);
        }
    }

    public void DropItemToPosition(Vector3 pos, GameObject ItemPrefab, List<GameObject> Items, int ranmin, int ranmax)
    {
        int RandomDrop;
        RandomDrop = Random.Range(ranmin, ranmax);
        //if (RandomDrop < 20 || RandomDrop > 95) return;
        GameObject reusedItem = null;
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].activeSelf == false)
            {
                reusedItem = Items[i];
                break;
            }
        }
        if (reusedItem == null)
        {
            GameObject newItem = Instantiate(ItemPrefab);
            Items.Add(newItem);
            reusedItem = newItem;
        }
        reusedItem.SetActive(true);
        reusedItem.transform.position = new Vector3(pos.x, reusedItem.transform.position.y, pos.z);
    }
}
