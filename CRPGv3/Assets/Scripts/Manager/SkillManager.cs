using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.EventSystems;

public class SkillManager : Singleton<SkillManager>
{
    public GameObject SkillMenu;
    public bool IsOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        SkillMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!IsOpen)
            {
                IsOpen = true;
                SkillMenu.SetActive(true);
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.Escape))
                {
                    IsOpen = false;
                    SkillMenu.SetActive(false);
                }
            }
        }
    }

    public void OpenSkill()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            SkillMenu.SetActive(true);
        }
        else
        {
            IsOpen = false;
            SkillMenu.SetActive(false);
        }
    }
}
