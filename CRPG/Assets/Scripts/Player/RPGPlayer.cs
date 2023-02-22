using ARPGFX;
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
        public Coroutine act = null;
        bool SkillPrepare = false;
        Transform mySkillPoint = null;
        int selectnum;
        [SerializeField]
        GameObject Holder;
        public Sword mySword;
        [SerializeField]
        CharacterInfo theCharacterInfo;
        [SerializeField]
        GameObject LevelUpEff;
        [SerializeField]
        GameObject DeadWindow;
        [SerializeField]
        SkillManager theSkillManager;
        [SerializeField]
        GameObject[] SkillRegions;
        [SerializeField]
        GameObject TargetingRegion;
        [SerializeField]
        GameObject NonTargetingRegion;

        public PlayerUI myUI;
        public enum STATE
        {
            Create, Play, Death
        }
        int Level = 1;
        int MaxLevel = 99;
        float remainExp;
        public STATE myState = STATE.Create;
        public Animator _myanim = null;
        public Transform _mytarget = null;
        public LayerMask pickMask = default;
        public LayerMask enemyMask = default;
        public Quest quest;
        public float orgRange;

        void ChangeState(STATE s)
        {
            if (myState == s) return;
            myState = s;
            switch (myState)
            {
                case STATE.Create:
                    break;
                case STATE.Play:
                    WeaponStat();
                    break;
                case STATE.Death:
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
                    _mytarget = myTarget;
                    mySword = Holder.GetComponentInChildren<Sword>();
                    UIText();
                    LevelUp();
                    PlayerMove();
                    UseSkill(KeyCode.Alpha1, 0);
                    UseSkill(KeyCode.Alpha2, 1);
                    UseSkill(KeyCode.Alpha3, 2);
                    UseSkill(KeyCode.Alpha4, 3);
                    StartCoroutine(SkillStart());
                    if (quest.isActive)
                    {
                        if (quest.goal.IsReached() || quest.goal.IsDone())
                        {
                            QuestManager.Inst.QuestionMark.SetActive(true);
                            QuestManager.Inst.UnfinishQuestionMark.SetActive(false);
                            QuestManager.Inst.ExclamationMark.SetActive(false);
                        }
                    }
                    break;
                case STATE.Death:
                    break;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            _myanim = myAnim;
            DeadWindow.SetActive(false);
            orgRange = myStat.AttackRange;
            LevelUpEff.SetActive(false);
            myStat.changeHp = (float v) => myUI.myHpBar.value = v;
            myStat.changeMp = (float v) => myUI.myMpBar.value = v;
            myStat.changeExp = (float v) => myUI.myExpBar.value = v;

            ChangeState(STATE.Play);
        }

        void UIText()
        {
            myUI.myLevelText.text = "레벨 " + Level.ToString();
            myUI.HP_Text.text = myStat.HP.ToString() + " / " + myStat.maxHp.ToString();
            myUI.MP_Text.text = myStat.MP.ToString() + " / " + myStat.maxMp.ToString();
            myUI.EXP_Text.text = (myStat.EXP / myStat.maxExp * 100).ToString() + " %";
            theCharacterInfo.HPText.text = myStat.HP.ToString() + " / " + myStat.maxHp.ToString();
            theCharacterInfo.MPText.text = myStat.MP.ToString() + " / " + myStat.maxMp.ToString();
            theCharacterInfo.APText.text = myStat.AP.ToString();
            theCharacterInfo.ADText.text = myStat.AttackDelay.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            StateProcess(); 
        }

        public void WeaponStat()
        {
            if (mySword != null)
            {
                mySkillPoint = mySword.mySkillPoint;
                myStat.AttackRange = myStat.AttackRange + mySword.range;
                orgRange = myStat.AttackRange;
                myStat.AttackDelay = myStat.AttackDelay - mySword.AttackSpeed;
                myStat.AP = myStat.AP + mySword.damage;
            }
            else
            {
                myStat.AttackRange = 1;
                myStat.AttackDelay = 1;
                myStat.AP = 5;
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
                    Level++;
                    LevelUpEff.SetActive(true);
                    remainExp = myStat.EXP - myStat.maxExp;
                    myStat.maxHp += 100;
                    myStat.maxMp += 100;
                    myStat.maxExp += 500;
                    myStat.HP = myStat.maxHp;
                    myStat.MP = myStat.maxMp;
                    myStat.EXP = remainExp;
                    myStat.AP += 5;
                }
                LevelUpEff.SetActive(false);
            }
        }

        public override void OnDamage(float dmg)
        {
            myStat.HP -= dmg;
            if (Mathf.Approximately(myStat.HP, 0.0f))
            {
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
                if (quest.isActive)
                {
                    quest.goal.EnemyKilled(myTarget.gameObject);
                }
                if (Level == MaxLevel)
                {
                    Mathf.Clamp(myStat.EXP, 0, myStat.maxExp - 1);
                }
                else
                {
                    myStat.EXP += myTarget.GetComponent<Monster>().GiveExp;
                }
            }
        }

        public void PlayerMove()
        {
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                //마우스 위치에서 내부의 가상공간으로 뻗어 나가는 레이를 만든다.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //레이어마스크에 해당하는 오브젝트가 선택 되었는지 확인 한다.
                if (Physics.Raycast(ray, out hit, 1000.0f, enemyMask))
                {
                    myTarget = hit.transform;
                    AttackTarget(myTarget);
                }
                else if (Physics.Raycast(ray, out hit, 1000.0f, pickMask))
                {
                    MoveToPositionByNav(hit.point);
                }
            }
        }

        IEnumerator SkillStart()
        {
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0))
            {
                if (SkillPrepare)
                {
                    if (theSkillManager.SkillSlots[selectnum].mySkillData.myType == SkillData.SkillType.Targeting)
                    {
                        if (myTarget != null)
                        {
                            GameObject TargetingSkill = Instantiate(theSkillManager.SkillSlots[selectnum].mySkillData.mySkill, mySkillPoint);
                            TargetingSkill.GetComponent<Projectile>().OnFire(myTarget, enemyMask, theSkillManager.SkillSlots[selectnum].mySkillDamage);
                            myAnim.SetTrigger("TargetingSkill");
                        }
                    }
                    else if (theSkillManager.SkillSlots[selectnum].mySkillData.myType == SkillData.SkillType.NonTargeting)
                    {
                        GameObject NonTargetingSkill = Instantiate(theSkillManager.SkillSlots[selectnum].mySkillData.mySkill);
                        NonTargetingSkill.GetComponent<Projectile>().OnFire(NonTargetingRegion.transform, pickMask, theSkillManager.SkillSlots[selectnum].mySkillDamage);
                        myAnim.SetTrigger("NonTargetingSkill");
                    }
                }
            }
            yield return null;
            yield return StartCoroutine(SkillStart());
            act = StartCoroutine(theSkillManager.SkillSlots[selectnum].Cooling());
            myAnim.SetBool("Skill", false);
            SkillPrepare = false;
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
                GameManager.Inst.Goldvalue -= 2000;
                myAnim.SetTrigger("Revive");
                ChangeState(STATE.Play);
                myStat.HP = myStat.maxHp;
                myStat.MP = myStat.maxMp;
            }
            else
            {
                StartCoroutine(GetComponent<ActionController>().WhenNoMoney());
            }
        }

        public void GoVillage()
        {
            myStat.HP = 1;
            myStat.MP = 1;
            if (myStat.EXP != 0)
            {
                if (myStat.EXP < 20)
                {
                    myStat.EXP = 0;
                }
                else
                {
                    myStat.EXP -= 20;
                }
            }
            else
            {
                myStat.EXP = 0;
            }
            SceneManager.LoadScene("Village");
        }

        void UseSkill(KeyCode alpha, int num)
        {
            if (Input.GetKeyDown(alpha))
            {
                if (theSkillManager.SkillSlots[num].mySkillData == null) return;
                if (act != null) return;
                SkillPrepare = true;
                if (theSkillManager.SkillSlots[num].mySkillData.myType == SkillData.SkillType.Targeting)
                {
                    TargetingRegion.SetActive(true);
                    NonTargetingRegion.SetActive(false);
                }
                else if (theSkillManager.SkillSlots[num].mySkillData.myType == SkillData.SkillType.NonTargeting)
                {
                    TargetingRegion.SetActive(false);
                    NonTargetingRegion.SetActive(true);
                }
                Debug.Log(theSkillManager.SkillSlots[num].mySkillData.myType);
                selectnum = num;
                myAnim.SetBool("Skill", true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SkillPrepare = false;
                myAnim.SetBool("Skill", false);
                TargetingRegion.SetActive(false);
                NonTargetingRegion.SetActive(false);
            }
        }
    }
}