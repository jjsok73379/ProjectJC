using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject orgMonster;
    List<GameObject> list = new List<GameObject>();
    [SerializeField]
    Vector3[] pos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(list.Count < 6)
        {
            pos[0].x = Random.Range(pos[0].x - 3.0f, pos[0].x + 3.0f);
            pos[0].y = 1.0f;
            pos[0].z = Random.Range(pos[0].z - 3.0f, pos[0].z + 3.0f);
            Vector3 rot = Vector3.zero;
            rot.y = Random.Range(0.0f, 360.0f);
            GameObject obj = Instantiate(orgMonster, pos[0], Quaternion.Euler(rot));
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
