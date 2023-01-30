using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData itemdata;
    public Sprite image;
    public float rotateSpeed = 180.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    public ItemData GetItem()
    {
        return itemdata;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            RemoveFromWorld();
        }
    }

    public void RemoveFromWorld()
    {
        InventoryManager.Inst.itemDB.Add(GetItem());
        InventoryManager.Inst.DataBase();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0.0f, rotateSpeed * Time.deltaTime, 0.0f);
    }
}
