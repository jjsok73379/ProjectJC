using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombinedSkill : MonoBehaviour
{
    public SkillData mySkillData;
    SkillManager skill;
    public Sprite orgImage;

    // Start is called before the first frame update
    void Start()
    {
        orgImage = GetComponent<Image>().sprite;
        skill = SkillManager.Inst;
    }

    public void AddToList()
    {
        skill.mySkill = mySkillData;
        skill.AddSkill();
        GetComponent<Image>().sprite = orgImage;
        skill.myCombinedSkillText.SetActive(true);
    }
}
