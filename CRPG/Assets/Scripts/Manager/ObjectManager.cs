using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public GameObject BookPrefab;
    public GameObject PotionPrefab;
    public GameObject CoinPrefab;
    public int initialItems = 10;
    public List<GameObject> books = new List<GameObject>();
    public List<GameObject> potions = new List<GameObject>();
    public List<GameObject> coins = new List<GameObject>();

    private void Awake()
    {
        MakeItems(BookPrefab, books);
        MakeItems(PotionPrefab, potions);
        MakeItems(CoinPrefab, coins);
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

    public void DropItemToPosition(Vector3 pos, GameObject ItemPrefab, List<GameObject> Items, int ranmin, int ranmax, int coinValue)
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
        if (ItemPrefab == CoinPrefab)
        {
            reusedItem.GetComponent<Coin>().SetCoinValue(coinValue);
        }
        reusedItem.transform.position = new Vector3(pos.x, reusedItem.transform.position.y, pos.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
