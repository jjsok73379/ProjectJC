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
        bool SkillPrepare = false;
        bool IsSkillStart = false;
        bool IsCombable = false;
        int clickCount = 0;
        int selectnum;
        ActionController theActionController;
        GameObject bombSkill;
        [SerializeField]
        Camera myCam;
        [SerializeField]
        GameObject Holder;
        public Sword mySword;
        [SerializeField]
        CharacterInfo theCharacterInfo;
        [SerializeField]
        ParticleSystem LevelUpEff;
        [SerializeField]
        GameObject DeadWindow;
        [SerializeField]
        SkillManager theSkillManager;
        [SerializeField]
        Transform mySkillPoint;
        public GameObject NonTargetingRegion;
        public PlayerUI myUI;
        public enum STATE
        {
            Create, Play, Death
        }
        int Level = 1;
        int MaxLevel = 99;
        float remainExp;
        public STATE myState = STATE.Create;
        public LayerMask pickMask = default;
        public LayerMask enemyMask = default;
        public Quest quest;
        public float orgRange;
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
                    NonTargetingRegion.SetActive(false);
                    DeadWindow.SetActive(false);
                    orgRange = myStat.AttackRange;
                    orgDamage = myStat.AP;
                    myStat.changeHp = (float v) => myUI.myHpBar.value = v;
                    myStat.changeMp = (float v) => myUI.myMpBar.value = v;
                    myStat.changeExp = (float v) => myUI.myExpBar.value = v;
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
                    if (quest.isActive)
                    {
                        if (quest.goal.IsReached() || quest.goal.IsDone())
                        {
                            QuestManager.Inst.QuestionMark.SetActive(true);
                            QuestManager.Inst.UnfinishQuestionMark.SetActive(false);
                            QuestManager.Inst.ExclamationMark.SetActive(false);
                        }
                    }
                    else return;
                    break;
                case STATE.Death:
                    break;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            theActionController = GetComponent<ActionController>();
            ChangeState(STATE.Play);
        }

        void UIText()
        {
            myUI.myLevelText.text = "���� " + Level.ToString();
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
                myStat.AP = myStat.AP + mySword.damage;
                orgDamage = myStat.AP;
            }
            else
            {
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
                    LevelUpEff.Play(true);
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
            if (!EventSystem.current.IsPointerOverGameObject() && Input.GetMouseButtonDown(0)
                && !myAnim.GetBool("IsSkill"))
            {
                //���콺 ��ġ���� ������ ����������� ���� ������ ���̸� �����.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                //���̾��ũ�� �ش��ϴ� ������Ʈ�� ���� �Ǿ����� Ȯ�� �Ѵ�.
                if (Physics.Raycast(ray, out hit, 1000.0f, enemyMask))
                {
                    myTarget = hit.transform;
                    if (SkillPrepare)
                    {
                        if (theSkillManager.SkillSlots[selectnum].mySkillData.myType == SkillData.SkillType.Ball)
                        {
                            if (myTarget != null)
                            {
                                transform.LookAt(myTarget.transform.position);
                                if (myStat.MP >= theSkillManager.SkillSlots[selectnum].mySkillData.Mana)
                                {
                                    myStat.MP -= theSkillManager.SkillSlots[selectnum].mySkillData.Mana;
                                    myAnim.SetTrigger("Ball");
                                    GameObject Projectile = Instantiate(theSkillManager.SkillSlots[selectnum].mySkillData.mySkill, mySkillPoint.position, mySkillPoint.rotation);
                                    Projectile.GetComponent<Projectile>().myTarget = myTarget.GetComponent<Monster>();
                                    Projectile.GetComponent<Skill>().SkillDamage = myStat.AP;
                                    Projectile.GetComponent<Skill>().OnFire();
                                }
                                else
                                {
                                    StartCoroutine(theActionController.WhenNoMana());
                                }
                            }
                        }
                        else if(theSkillManager.SkillSlots[selectnum].mySkillData.myType == SkillData.SkillType.Bomb)
                        {
                            if (myTarget != null)
                            {
                                if(myStat.MP >= theSkillManager.SkillSlots[selectnum].mySkillData.Mana)
                                {
                                    myStat.MP -= theSkillManager.SkillSlots[selectnum].mySkillData.Mana;
                                    myAnim.SetTrigger("Bomb");
                                }
                            }
                        }
                    }
                }
                else if (Physics.Raycast(ray, out hit, 1000.0f, pickMask))
                {
                    if (SkillPrepare)
                    {
                        if (theSkillManager.SkillSlots[selectnum].mySkillData.myType == SkillData.SkillType.NonTargeting)
                        {
                            if (myStat.MP >= theSkillManager.SkillSlots[selectnum].mySkillData.Mana)
                            {
                                myStat.MP -= theSkillManager.SkillSlots[selectnum].mySkillData.Mana;
                                myAnim.SetTrigger("NonTargetingSkill");
                                GameObject NonTargeting = Instantiate(theSkillManager.SkillSlots[selectnum].mySkillData.mySkill, NonTargetingRegion.transform.position, Quaternion.Euler(-270, 0, 0));
                                NonTargeting.GetComponent<Skill>().SkillDamage = myStat.AP;
                                NonTargeting.GetComponent<Skill>().OnFire();
                            }
                            else
                            {
                                StartCoroutine(theActionController.WhenNoMana());
                            }
                        }
                    }
                    else
                    {
                        MoveToPositionByNav(hit.point);
                    }
                }
            }
        }

        public void BombSkill()
        {
            bombSkill = Instantiate(theSkillManager.SkillSlots[selectnum].mySkillData.mySkill, myTarget.position, Quaternion.identity);
            if (bombSkill.name == "IceBomb(Clone)")
            {
                myTarget.GetComponent<Monster>().AddDebuff(Debuff.Type.Slow, 0.5f, 1.0f);
            }
            else if (bombSkill.name == "ThunderBomb(Clone)")
            {
                myTarget.GetComponent<Monster>().AddDebuff(Debuff.Type.Stun, 0.3f, 0.4f);
            }
            myTarget.GetComponent<Monster>().OnDamage(myStat.AP);
        }

        public void ComboAttack()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !myAnim.GetBool("IsComboAttacking"))
            {
                if (!SkillPrepare)
                {
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
            Collider[] list = Physics.OverlapSphere(mySword.myAttackPoint.position, 1f, enemyMask);

            foreach (Collider col in list)
            {
                col.GetComponent<IBattle>()?.OnDamage(myStat.AP);
            }
        }

        public void ComboCheck(bool v)
        {
            if (v)
            {
                // �޺� ����
                IsCombable = true;
                clickCount = 0;
            }
            else
            {
                // �޺� ��
                IsCombable = false;
                if (clickCount == 0)
                {
                    myAnim.SetTrigger("ComboFail");
                }
            }
        }

        IEnumerator SkillEnd()
        {
            theSkillManager.SkillSlots[selectnum].act = StartCoroutine(theSkillManager.SkillSlots[selectnum].Coolact);
            StartCoroutine(theSkillManager.SkillSlots[selectnum].Coolact);
            myStat.AttackRange = orgRange;
            myStat.AP = orgDamage;
            myAnim.SetBool("Skill", false);
            Destroy(bombSkill);
            if (theSkillManager.SkillSlots[selectnum].mySkillData.myType == SkillData.SkillType.NonTargeting)
            {
                NonTargetingRegion.SetActive(false);
            }
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
            if (theSkillManager.SkillSlots[num].mySkillData == null) return;
            if (theSkillManager.SkillSlots[num].act != null) return;
            if (Input.GetKeyDown(alpha))
            {
                SkillPrepare = true;
                myStat.AttackRange = theSkillManager.SkillSlots[num].mySkillRange;
                myStat.AP = theSkillManager.SkillSlots[num].mySkillDamage + myStat.AP;
                if (theSkillManager.SkillSlots[num].mySkillData.myType == SkillData.SkillType.NonTargeting)
                {
                    NonTargetingRegion.SetActive(true);
                }
                selectnum = num;
                myAnim.SetBool("Skill", true);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SkillPrepare = false;
                myStat.AttackRange = orgRange;
                myStat.AP = orgDamage;
                myAnim.SetBool("Skill", false);
                if (theSkillManager.SkillSlots[num].mySkillData.myType == SkillData.SkillType.NonTargeting)
                {
                    NonTargetingRegion.SetActive(false);
                }
            }
        }
    }
}