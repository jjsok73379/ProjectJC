using ARPGFX;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace CombineRPG
{
    public class RPGPlayer : BattleSystem
    {
        Coroutine[] act = new Coroutine[3];
        bool SkillPrepare = false;
        bool IsSkillStart = false;
        bool IsCombable = false;
        int clickCount = 0;
        int selectnum;
        ActionController theActionController;
        GameObject bombSkill;
        [SerializeField]
        GameObject Holder;
        public Sword mySword;
        [SerializeField]
        CharacterInfo theCharacterInfo;
        [SerializeField]
        ParticleSystem LevelUpEff;
        [SerializeField]
        ParticleSystem ReviveEff;
        [SerializeField]
        Transform AttackPoint;
        [SerializeField]
        GameObject DeadWindow;
        [SerializeField]
        GameObject mousePoint;
        [SerializeField]
        SkillSlot[] SkillSlots = new SkillSlot[4];
        Coroutine mouseClick = null;
        public Transform mySkillPoint;
        public GameObject NonTargetingRegion;
        public PlayerUI myUI;
        public GameObject questobj;
        public int i = 0;

        public enum STATE
        {
            Create, Play, Death
        }
        public int Level = 1;
        public int MaxLevel = 99;
        float remainExp;
        public STATE myState = STATE.Create;
        public LayerMask pickMask = default;
        public LayerMask enemyMask = default;
        public Quest quest;
        public float orgDamage;
        Coroutine SkillChk;

        void ChangeState(STATE s)
        {
            if (myState == s) return;
            myState = s;
            switch (myState)
            {
                case STATE.Create:
                    break;
                case STATE.Play:
                    for(int i = 0; i < DataManager.Inst.SkillSlotDatas.Length;)
                    {
                        if (DataManager.Inst.SkillSlotDatas.Length > 0 && SkillSlots[i].mySkillData == null)
                        {
                            SkillSlots[i].mySkillData = DataManager.Inst.SkillSlotDatas[i];
                        }
                        i++;
                    }
                    if (quest.isActive)
                    {
                        questobj = Instantiate(Resources.Load("Prefabs/Quest/QuestContent"), OpenManager.Inst.go_Quest.transform.GetChild(1)) as GameObject;
                        for (int j = 0; j < questobj.GetComponent<QuestInfo>().titleText.Length; j++)
                        {
                            questobj.GetComponent<QuestInfo>().titleText[j].text = quest.title;
                        }
                        questobj.GetComponent<QuestInfo>().descriptionText.text = quest.description;
                        questobj.GetComponent<QuestInfo>().EXP_text.text = quest.experienceReward.ToString();
                        questobj.GetComponent<QuestInfo>().goldText.text = quest.goldReward.ToString();
                        questobj.GetComponent<QuestInfo>().AfterAccept();
                    }
                    NonTargetingRegion.SetActive(false);
                    DeadWindow.SetActive(false);
                    mousePoint.SetActive(false);
                    orgDamage = myStat.AP;
                    myStat.changeHp = (float v) => myUI.myHpBar.value = v;
                    myStat.changeMp = (float v) => myUI.myMpBar.value = v;
                    myStat.changeExp = (float v) => myUI.myExpBar.value = v;
                    WeaponStat();
                    break;
                case STATE.Death:
                    myUI.HP_Text.text = "0 / 100";
                    StopAllCoroutines();
                    myAnim.SetTrigger("Dead");
                    foreach (IBattle ib in myAttackers)
                    {
                        ib.DeadMessage(transform);
                    }
                    DeadWindow.SetActive(true);
                    break;
            }
        }
        void StateProcess()
        {
            switch (myState)
            {
                case STATE.Create:
                    break;
                case STATE.Play:
                    UIText();
                    LevelUp();
                    PlayerMove();
                    ComboAttack();
                    UseSkill(KeyCode.Alpha1, 0);
                    UseSkill(KeyCode.Alpha2, 1);
                    UseSkill(KeyCode.Alpha3, 2);
                    UseSkill(KeyCode.Alpha4, 3);
                    if (IsSkillStart)
                    {
                        StartCoroutine(SkillEnd());
                    }
                    if (SkillPrepare)
                    {
                        CursorManager.Inst.SetActiveCursorType(CursorManager.CursorType.Magic);
                    }
                    else
                    {
                        CursorManager.Inst.SetActiveCursorType(CursorManager.CursorType.Arrow);
                    }
                    if (QuestManager.Inst != null)
                    {
                        if (quest != null)
                        {
                            if (quest.isActive)
                            {
                                DataManager.Inst.IsQuesting = true;
                                if (quest.goal.IsReached() || quest.goal.IsDone())
                                {
                                    QuestManager.Inst.QuestionMark.SetActive(true);
                                    QuestManager.Inst.UnfinishQuestionMark.SetActive(false);
                                    QuestManager.Inst.ExclamationMark.SetActive(false);
                                }
                            }
                        }
                    }
                    else
                    {
                        DataManager.Inst.IsFinishQuest = true;
                    }
                    break;
                case STATE.Death:
                    break;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            myUI = DontDestroyUI.Inst.GetComponentInChildren<PlayerUI>();
            theActionController = GetComponent<ActionController>();
            ChangeState(STATE.Play);
        }

        void UIText()
        {
            myUI.myLevelText.text = "레벨 " + Level.ToString();
            myUI.HP_Text.text = myStat.HP.ToString() + " / " + myStat.maxHp.ToString();
            myUI.MP_Text.text = myStat.MP.ToString() + " / " + myStat.maxMp.ToString();
            myUI.EXP_Text.text = (myStat.EXP / myStat.maxExp * 100).ToString("0.0") + " %";
            theCharacterInfo.HPText.text = myStat.HP.ToString() + " / " + myStat.maxHp.ToString();
            theCharacterInfo.MPText.text = myStat.MP.ToString() + " / " + myStat.maxMp.ToString();
            theCharacterInfo.APText.text = myStat.AP.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            StateProcess();
        }

        IEnumerator mousepointer(Vector3 pos)
        {
            SoundManager.Inst.MoveClickSound.Play();
            float i = 0.0f;
            mousePoint.SetActive(true);
            mousePoint.transform.position = pos;
            while (i < 1.0f)
            {
                i += 0.1f;
                yield return new WaitForSeconds(0.1f);
                mousePoint.SetActive(false);
            }
        }

        public void WeaponStat()
        {
            if (mySword != null)
            {
                myStat.AP = Level * 5 + mySword.damage;
                orgDamage = myStat.AP;
            }
            else
            {
                myStat.AP = Level * 5;
            }
        }

        void LevelUp()
        {
            if (myStat.EXP >= myStat.maxExp)
            {
                if (Level == MaxLevel)
                {
                    return;
                }
                else
                {
                    SoundManager.Inst.LVUpSound.Play();
                    Level++;
                    LevelUpEff.Play();
                    remainExp = myStat.EXP - myStat.maxExp;
                    myStat.maxHp += 100;
                    myStat.maxMp += 100;
                    myStat.maxExp += 500;
                    myStat.HP = myStat.maxHp;
                    myStat.MP = myStat.maxMp;
                    myStat.EXP = remainExp;
                    myStat.AP += 5;
                }
            }
        }

        public override void OnDamage(float dmg)
        {
            myStat.HP -= dmg;
            if (Mathf.Approximately(myStat.HP, 0.0f))
            {
                myStat.HP = 0;
                ChangeState(STATE.Death);
            }
            else
            {
                myAnim.SetTrigger("Damage");
            }
        }

        public override bool IsLive()
        {
            return !Mathf.Approximately(myStat.HP, 0.0f);
        }

        public override void DeadMessage(Transform tr)
        {
            if (tr == myTarget)
            {
                StopAllCoroutines();
            }
        }

        public void PlayerMove()
        {
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0)
                && !myAnim.GetBool("IsSkill"))
            {
                //마우스 위치에서 내부의 가상공간으로 뻗어 나가는 레이를 만든다.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //레이어마스크에 해당하는 오브젝트가 선택 되었는지 확인 한다.
                if (Physics.Raycast(ray, out hit, 1000.0f, enemyMask))
                {
                    myTarget = hit.transform;
                    if (SkillPrepare)
                    {
                        if (SkillSlots[selectnum].mySkillData.myType == SkillData.SkillType.Ball)
                        {
                            if (myTarget != null)
                            {
                                CoolTIme_end(0);
                                transform.LookAt(myTarget.transform.position);
                                if (myStat.MP >= SkillSlots[selectnum].mySkillData.Mana)
                                {
                                    myStat.MP -= SkillSlots[selectnum].mySkillData.Mana;
                                    myAnim.SetTrigger("Ball");
                                    GameObject Projectile = Instantiate(SkillSlots[selectnum].mySkillData.mySkill, mySkillPoint.position, mySkillPoint.rotation);
                                    Projectile.GetComponent<Projectile>().myTarget = myTarget.GetComponent<Monster>();
                                    Projectile.GetComponent<Skill>().SkillDamage = myStat.AP;
                                    Projectile.GetComponent<Skill>().OnFire();
                                }
                                else
                                {
                                    StartCoroutine(SkillEnd());
                                    StartCoroutine(theActionController.WhenNoMana());
                                }
                            }
                        }
                        else if(SkillSlots[selectnum].mySkillData.myType == SkillData.SkillType.Bomb)
                        {
                            CoolTIme_end(1);
                            if (myTarget != null)
                            {
                                if(myStat.MP >= SkillSlots[selectnum].mySkillData.Mana)
                                {
                                    myStat.MP -= SkillSlots[selectnum].mySkillData.Mana;
                                    myAnim.SetTrigger("Bomb");
                                }
                            }
                            else
                            {
                                StartCoroutine(SkillEnd());
                                StartCoroutine(theActionController.WhenNoMana());
                            }
                        }
                    }
                }
                else if (Physics.Raycast(ray, out hit, 1000.0f, pickMask))
                {
                    if (SkillPrepare)
                    {
                        if (SkillSlots[selectnum].mySkillData.myType == SkillData.SkillType.NonTargeting)
                        {
                            CoolTIme_end(2);
                            if (myStat.MP >= SkillSlots[selectnum].mySkillData.Mana)
                            {
                                OpenManager.Inst.IsActive = false;
                                myStat.MP -= SkillSlots[selectnum].mySkillData.Mana;
                                NonTargetingRegion.SetActive(false);
                                myAnim.SetTrigger("NonTargetingSkill");
                                GameObject NonTargeting = Instantiate(SkillSlots[selectnum].mySkillData.mySkill, NonTargetingRegion.transform.position, NonTargetingRegion.transform.rotation);
                                NonTargeting.GetComponent<Skill>().SkillDamage = myStat.AP;
                                NonTargeting.GetComponent<Skill>().OnFire();
                            }
                            else
                            {
                                StartCoroutine(SkillEnd());
                                StartCoroutine(theActionController.WhenNoMana());
                            }
                        }
                    }
                    else
                    {
                        if (mouseClick != null)
                        {
                            StopCoroutine(mouseClick);
                            mouseClick = null;
                        }
                        mouseClick = StartCoroutine(mousepointer(hit.point));
                        MoveToPositionByNav(hit.point);
                    }
                }
            }
        }

        public void BombSkill()
        {
            bombSkill = Instantiate(SkillSlots[selectnum].mySkillData.mySkill, myTarget.position, Quaternion.identity);
            if (bombSkill.name == "IceBomb(Clone)")
            {
                SoundManager.Inst.IceSkillSound.Play();
                myTarget.GetComponent<Monster>().AddDebuff(Debuff.Type.Slow, 0.5f, 1.0f);
            }
            else if (bombSkill.name == "ThunderBomb(Clone)")
            {
                SoundManager.Inst.ThunderSkillSound.Play();
                myTarget.GetComponent<Monster>().AddDebuff(Debuff.Type.Stun, 0.3f, 0.4f);
            }
            else
            {
                SoundManager.Inst.BasicSkillSound.Play();
            }
            myTarget.GetComponent<Monster>().OnDamage(myStat.AP);
        }

        public void ComboAttack()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !myAnim.GetBool("IsComboAttacking"))
            {
                if (!SkillPrepare)
                {
                    SoundManager.Inst.SwordAttackSound.Play();
                    myAnim.SetTrigger("ComboAttack");
                }
            }
            if (IsCombable)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ++clickCount;
                }
            }
        }

        public void ComboAttackTarget()
        {
            Collider[] list = Physics.OverlapSphere(AttackPoint.position, 1f, enemyMask);

            if (list.Length > 0)
            {
                foreach (Collider col in list)
                {
                    col.GetComponent<IBattle>()?.OnDamage(myStat.AP);
                }
            }
        }

        public void ComboCheck(bool v)
        {
            if (v)
            {
                // 콤보 시작
                IsCombable = true;
                clickCount = 0;
            }
            else
            {
                // 콤보 끝
                IsCombable = false;
                if (clickCount == 0)
                {
                    myAnim.SetTrigger("ComboFail");
                }
            }
        }

        public void CoolTIme_end(int i)
        {
            if (act[i] != null)
            {
                StopCoroutine(act[i]);
                act[i] = null;
            }
            act[i] = SkillSlots[selectnum].StartCoroutine(SkillSlots[selectnum].Cooling());
        }

        IEnumerator SkillEnd()
        {
            myStat.AP = orgDamage;
            myAnim.SetBool("Skill", false);
            Destroy(bombSkill);
            SkillPrepare = false;
            yield return null;
        }

        public void Skill_end()
        {
            if (SkillChk != null)
            {
                StopCoroutine(SkillChk);
                SkillChk = null;
            }
            SkillChk = StartCoroutine(SkillEnd());
        }

        public void IncreaseHP(int _count)
        {
            if (myStat.HP + _count < myStat.maxHp)
            {
                myStat.HP += _count;
            }
            else
            {
                myStat.HP = myStat.maxHp;
            }
        }

        public void IncreaseMP(int _count)
        {
            if (myStat.MP + _count < myStat.maxMp)
            {
                myStat.MP += _count;
            }
            else
            {
                myStat.MP = myStat.maxMp;
            }
        }

        public void Revive()
        {
            if (GameManager.Inst.Goldvalue >= 2000)
            {
                DeadWindow.SetActive(false);
                GameManager.Inst.Goldvalue -= 2000;
                myAnim.SetTrigger("Revive");
                ReviveEff.Play();
                myStat.HP = myStat.maxHp;
                myStat.MP = myStat.maxMp;
                ChangeState(STATE.Play);
            }
            else
            {
                StartCoroutine(GetComponent<ActionController>().WhenNoMoney());
            }
        }

        public void GoVillage()
        {
            DeadWindow.SetActive(false);
            DataManager.Inst.HP = 1;
            ChangeState(STATE.Play);
            LoadManager.LoadScene(1);
        }

        void UseSkill(KeyCode alpha, int num)
        {
            if (SkillSlots[num].mySkillData == null) return;
            if (SkillSlots[num].IsCooling) return;
            if (Input.GetKeyDown(alpha))
            {
                SkillPrepare = true;
                myStat.AttackRange = SkillSlots[num].mySkillRange;
                myStat.AP = SkillSlots[num].mySkillDamage + myStat.AP;
                if (SkillSlots[num].mySkillData.myType == SkillData.SkillType.NonTargeting)
                {
                    NonTargetingRegion.SetActive(true);
                }
                selectnum = num;
                myAnim.SetBool("Skill", true);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                SkillPrepare = false;
                myStat.AP = orgDamage;
                myAnim.SetBool("Skill", false);
                if (SkillSlots[num].mySkillData.myType == SkillData.SkillType.NonTargeting)
                {
                    NonTargetingRegion.SetActive(false);
                }
            }
        }

        public void DoQuest(int i)
        {

            if (quest != null)
            {
                if (quest.isActive)
                {
                    if (QuestManager.Inst != null)
                    {
                        if (QuestManager.Inst.i == i)
                        {
                            quest.goal.IsDoAction();
                        }
                    }
                    else
                    {
                        if (this.i == i)
                        {
                            quest.goal.IsDoAction();
                        }
                    }
                }
            }
        }
    }
}