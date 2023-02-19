using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReinforceSkill : MonoBehaviour
{
    public static ReinforceSkill Inst;
    public SkillData ReinfoceData;
    [SerializeField]
    Image ReinforceImage;
    [SerializeField]
    MaterialSlot[] theMaterialSlots;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReinforceImage.sprite = ReinfoceData.myImage;
    }
}
