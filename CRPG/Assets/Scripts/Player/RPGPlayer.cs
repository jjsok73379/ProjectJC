using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombineRPG
{
    public class RPGPlayer : BattleSystem
    {
        public PlayerUI myUI;
        public enum STATE
        {
            Create, Play, Death
        }
        int Level = 98;
        int MaxLevel = 99;
        float remainExp;
        public STATE myState = STATE.Create;
        public Animator _myanim = null;
        public Transform _mytarget = null;
        public LayerMask pickMask = default;
        public LayerMask enemyMask = default;
        public Transform mySkillPoint = null;

        MinimapIcon myIcon = null;

        void ChangeState(STATE s)
        {
            if (myState == s) return;
            myState = s;
            switch (myState)
            {
                case STATE.Create:
                    break;
                case STATE.Play:
                    break;
                case STATE.Death:
                    StopAllCoroutines();
                    myAnim.SetTrigger("Dead");
                    foreach (IBattle ib in myAttackers)
                    {
                        ib.DeadMessage(transform);
                    }
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
                    UIText();
                    LevelUp();
                    PlayerMove();
                    break;
                case STATE.Death:
                    break;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            _myanim = myAnim;
            GameObject obj = Instantiate(Resources.Load("Prefabs/MinimapIcon"), GameManager.Inst.Minimap) as GameObject;
            myIcon = obj.GetComponent<MinimapIcon>();
            myIcon.Initialize(transform, Color.green);

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
            myUI.EXP_Text.text = (myStat.EXP/myStat.maxExp * 100).ToString() + " %";
        }

        // Update is called once per frame
        void Update()
        {
            StateProcess(); 
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
                    MoveToPosition(hit.point);
                }
            }
        }
    }
}