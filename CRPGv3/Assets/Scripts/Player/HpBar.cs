using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public Transform myTarget;
    public Slider myBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(myTarget.position);
        if (pos.z < 0.0f)
        {
            transform.position = new Vector3(0, 10000, 0);
        }
        else
        {
            transform.position = pos;
        }
    }
}
