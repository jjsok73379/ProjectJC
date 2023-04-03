using CombineRPG;
using Mono.Cecil.Cil;
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

    public void BeforeAccept()
    {
        AcceptBtn.gameObject.SetActive(true);
        RefuseBtn.gameObject.SetActive(true);
        CompleteBtn.gameObject.SetActive(false);
        ForgiveBtn.gameObject.SetActive(false);
        QuestManager.Inst.QuestionMark.SetActive(false);
        QuestManager.Inst.UnfinishQuestionMark.SetActive(false);
        QuestManager.Inst.ExclamationMark.SetActive(true);
    }

    public void AfterAccept()
    {
        AcceptBtn.gameObject.SetActive(false);
        RefuseBtn.gameObject.SetActive(false);
        CompleteBtn.gameObject.SetActive(true);
        ForgiveBtn.gameObject.SetActive(true); 
        if(QuestManager.Inst != null)
        {
            QuestManager.Inst.QuestionMark.SetActive(false);
            QuestManager.Inst.UnfinishQuestionMark.SetActive(true);
            QuestManager.Inst.ExclamationMark.SetActive(false);
        }
    }

    public void OpenInfo()
    {
        SoundManager.Inst.ButtonSound.Play();
        myInfo.SetActive(true);
    }

    public void ClickRefuse()
    {
        SoundManager.Inst.ButtonSound.Play();
        myInfo.SetActive(false);
    }

    public void CompleteQuest()
    {
        if (theRPGPlayer.quest.isActive)
        {
            if (theRPGPlayer.quest.goal.IsReached() || theRPGPlayer.quest.goal.IsDone())
            {
                SoundManager.Inst.CompleteQuestSound.Play();
                theRPGPlayer.myStat.EXP += theRPGPlayer.quest.experienceReward;
                GameManager.Inst.Goldvalue += theRPGPlayer.quest.goldReward;
                if(QuestManager.Inst != null)
                {
                    Destroy(QuestManager.Inst.objQ);
                    Destroy(QuestManager.Inst.objQ2);
                }
                else
                {
                    Destroy(theRPGPlayer.questobj);
                }
                DataManager.Inst.IsQuesting = false;
                theRPGPlayer.quest.Complete();
                theRPGPlayer.quest = null;
                DontDestroyUI.Inst.QuestPost.SetActive(false);
            }
            else
            {
                StartCoroutine(theActionController.WhenNotCompleted());
            }
        }
    }

    public void CloseBtn()
    {
        SoundManager.Inst.ButtonSound.Play();
        myInfo.SetActive(false);
    }
}
