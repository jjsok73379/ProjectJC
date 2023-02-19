using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestInfo : MonoBehaviour
{
    public TMP_Text[] titleText;
    public TMP_Text descriptionText;
    public TMP_Text EXP_text;
    public TMP_Text goldText;
    public GameObject myInfo;

    [SerializeField]
    Button AcceptBtn;
    [SerializeField]
    Button RefuseBtn;
    [SerializeField]
    Button CompleteBtn;
    [SerializeField]
    Button ForgiveBtn;
    ActionController theActionController;
    RPGPlayer theRPGPlayer;

    // Start is called before the first frame update
    void Start()
    {
        theActionController = ActionController.Inst;
        theRPGPlayer = theActionController.GetComponent<RPGPlayer>();
        gameObject.SetActive(true);
        myInfo.SetActive(false);
        AcceptBtn.onClick.AddListener(QuestManager.Inst.AcceptQuest);
        ForgiveBtn.onClick.AddListener(QuestManager.Inst.ForgiveQuest);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeforeAccept()
    {
        AcceptBtn.gameObject.SetActive(true);
        RefuseBtn.gameObject.SetActive(true);
        CompleteBtn.gameObject.SetActive(false);
        ForgiveBtn.gameObject.SetActive(false);
    }

    public void AfterAccept()
    {
        AcceptBtn.gameObject.SetActive(false);
        RefuseBtn.gameObject.SetActive(false);
        CompleteBtn.gameObject.SetActive(true);
        ForgiveBtn.gameObject.SetActive(true);
    }

    public void OpenInfo()
    {
        myInfo.SetActive(true);
    }

    public void ClickRefuse()
    {
        myInfo.SetActive(false);
    }

    public void CompleteQuest()
    {
        if (theRPGPlayer.quest != null)
        {
            if (theRPGPlayer.quest.goal.IsReached() || theRPGPlayer.quest.goal.IsDone())
            {
                theRPGPlayer.myStat.EXP += theRPGPlayer.quest.experienceReward;
                GameManager.Inst.Goldvalue += theRPGPlayer.quest.goldReward;
                Destroy(QuestManager.Inst.objQ);
                Destroy(QuestManager.Inst.objQ2);
                QuestManager.Inst.theQuestPost.gameObject.SetActive(false);
                QuestManager.Inst.theQuestPost.NoQuest();
                theRPGPlayer.quest.Complete();
                theRPGPlayer.quest = null;
            }
            else
            {
                StartCoroutine(theActionController.WhenNotCompleted());
            }
        }
    }

    public void CloseBtn()
    {
        myInfo.SetActive(false);
    }
}
