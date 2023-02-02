using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedSkill : MonoBehaviour
{
    public SkillData myData;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().sprite = myData.myImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
