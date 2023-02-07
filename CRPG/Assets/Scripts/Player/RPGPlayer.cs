using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CombineRPG
{
    public class RPGPlayer : BattleSystem
    {
        public int curExp { get; set; }
        public int expToNextLevel { get; set; }
        public int money { get; set; }
        //public PlayerUI myUI;
        public enum STATE
        {
            Create, Play, Death
        }
        public STATE myState = STATE.Create;
        public Animator _myanim = null;
        public Transform _mytarget = null;
        public LayerMask pickMask = default;
        public LayerMask enemyMask = default;
        public Transform mySkillPoint = null;

        //MinimapIcon myIcon = null;

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
                    PlayerMove();
                    /*
                    if(Input.GetMouseButtonDown(1) && !myAnim.GetBool("IsAttacking"))
                    {
                        myAnim.SetTrigger("Attack");
                    }
                    */
                    break;
                case STATE.Death:
                    break;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            _myanim = myAnim;
            /*
            Vector3 testDir = new Vector3(1, 1, 1);
            Debug.Log(testDir.magnitude);
            Vector3 temp = testDir.normalized;
            Debug.Log(testDir.magnitude);
            Debug.Log(temp.magnitude);
            */
            /*GameObject obj = Instantiate(Resources.Load("Prefabs/MinimapIcon"), SceneData.Inst.Minimap) as GameObject;
            myIcon = obj.GetComponent<MinimapIcon>();
            myIcon.Initialize(transform, Color.green);*/

            //myStat.changeHp = (float v) => myUI.myHpBar.value = v;
            ChangeState(STATE.Play);
        }

        // Update is called once per frame
        void Update()
        {
            StateProcess();
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

        public void AddMoney(int money)
        {
            this.money += money;
        }
    }
}