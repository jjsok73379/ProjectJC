using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : Singleton<ObjectManager>
{
    public GameObject BookPrefab;
    public GameObject PotionPrefab;
    public int initialItems = 10;
    public List<GameObject> books = new List<GameObject>();
    public List<GameObject> potions = new List<GameObject>();

    private void Awake()
    {
        MakeItems(BookPrefab, books);
        MakeItems(PotionPrefab, potions);
    }

    void MakeItems(GameObject ItemPrefab, List<GameObject> Items)
    {
        for(int i=0;i< initialItems; i++)
        {
            GameObject tempItem = Instantiate(ItemPrefab) as GameObject;
            tempItem.transform.parent = transform;
            tempItem.SetActive(false);
            Items.Add(tempItem);
        }
    }

    public void DropItemToPosition(Vector3 pos, GameObject ItemPrefab, List<GameObject> Items)
    {
        Debug.Log(ItemPrefab.name);
        GameObject reusedItem = null;
        for(int i=0;i< Items.Count; i++)
        {
            if (Items[i].activeSelf == false)
            {
                reusedItem = Items[i];
                break;
            }
        }
        if (reusedItem == null)
        {
            GameObject newBook = Instantiate(ItemPrefab) as GameObject;
            Items.Add(newBook);
            reusedItem = newBook;
        }
        reusedItem.SetActive(true);
        reusedItem.transform.position = new Vector3(pos.x, reusedItem.transform.position.y + 2.0f, pos.z);
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
