using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject orgMonster;
    List<GameObject> list = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(list.Count < 6)
        {
            Vector3 pos = new Vector3(195.34f, 0.0f, 186.31f);
            pos.x = Random.Range(188.0f, 202.0f);
            pos.y = 0.5f;
            pos.z = Random.Range(176.0f, 188.0f);
            Vector3 rot = Vector3.zero;
            rot.y = Random.Range(0.0f, 360.0f);
            GameObject obj = Instantiate(orgMonster, pos, Quaternion.Euler(rot));
            list.Add(obj);
        }

        for(int i = 0; i < list.Count;)
        {
            if(list[i] == null)
            {
                list.RemoveAt(i);
                continue;
            }
            ++i;
        }
    }
}
