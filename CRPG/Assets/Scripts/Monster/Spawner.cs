using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject orgMonster;
    [SerializeField]
    AudioSource myAttackSound;
    List<GameObject> list = new List<GameObject>();
    [SerializeField]
    Vector3 pos;
    [SerializeField]
    int CountNum;
    // Start is called before the first frame update
    void Start()
    {
        orgMonster.GetComponent<Monster>().myAttackSound = myAttackSound;
    }

    // Update is called once per frame
    void Update()
    {
        if(list.Count < CountNum)
        {
            pos.x = Random.Range(pos.x - 3.0f, pos.x + 3.0f);
            pos.y = 1.0f;
            pos.z = Random.Range(pos.z - 3.0f, pos.z + 3.0f);
            Vector3 rot = Vector3.zero;
            rot.y = Random.Range(0.0f, 360.0f);
            GameObject obj = Instantiate(orgMonster, pos, Quaternion.Euler(rot), transform);
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
