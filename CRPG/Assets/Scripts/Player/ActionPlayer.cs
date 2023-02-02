using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ActionPlayer : BattleSystem
{
    public enum STATE
    {
        Create, Play, Death
    }
    public List<Transform> myEnemys = new List<Transform>();
    public Monster _targeting = null;
    public Transform myAttackPoint;
    public Transform mySkillPoint;
    public STATE myState = STATE.Create;
    public GameObject Cam;
    public LayerMask myEnemyMask;
    public LayerMask layer;
    public float smoothMoveSpeed = 10.0f;
    Vector3 targetDir = Vector3.zero;
    bool IsCombable = false;
    int clickCount = 0;
    public float jumpHeight = 10.0f;
    public float rotSpeed = 360.0f;
    bool OnGround = false;
    public bool IsRun = false;

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
                PlayerMove();
                PlayerAttack();
                /*StartCoroutine(BasicLaserAttack());*/
                break;
            case STATE.Death:
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeState(STATE.Play);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    public void PlayerMove()
    {
        if (!myAnim.GetBool("IsAttacking"))
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                var offset = Cam.transform.forward;
                offset.y = 0;
                transform.LookAt(transform.position + offset);
            }
            targetDir.x = Input.GetAxisRaw("Horizontal");
            targetDir.y = Input.GetAxisRaw("Vertical");

            float x = Mathf.Lerp(myAnim.GetFloat("x"), targetDir.x, Time.deltaTime * smoothMoveSpeed);
            float z = Mathf.Lerp(myAnim.GetFloat("z"), targetDir.y, Time.deltaTime * smoothMoveSpeed);
            myAnim.SetFloat("x", x);
            myAnim.SetFloat("z", z);

            targetDir.Normalize();

            CheckGround();

            if (Input.GetButtonDown("Jump") && OnGround && !IsRun)
            {
                myAnim.SetTrigger("OnJump");
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                IsRun = true;
                myAnim.SetBool("IsMoving", true);
                smoothMoveSpeed = 20.0f;
                float Runx = Mathf.Lerp(myAnim.GetFloat("Runx"), targetDir.x, Time.deltaTime * smoothMoveSpeed);
                float Runz = Mathf.Lerp(myAnim.GetFloat("Runz"), targetDir.y, Time.deltaTime * smoothMoveSpeed);
                myAnim.SetFloat("Runx", Runx);
                myAnim.SetFloat("Runz", Runz);
            }
            else
            {
                IsRun = false;
                myAnim.SetFloat("Runx", 0);
                myAnim.SetFloat("Runz", 0);
                myAnim.SetBool("IsMoving", false);
            }
        }
    }

    public void PlayerAttack()
    {
        if (!myAnim.GetBool("IsAttacking"))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                myAnim.SetTrigger("Attack");
                AttackT();
            }
            if (IsCombable)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ++clickCount;
                }
            }
        }
    }

    /*IEnumerator BasicLaserAttack()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            myAnim.SetTrigger("Skill");
            if (myTarget != null)
            {
                GameObject obj = Instantiate(Resources.Load("SkillPrefabs/BasicLaser"), mySkillPoint.position, Quaternion.identity) as GameObject;
                obj.GetComponent<Projectile>().OnFire(myTarget, myEnemyMask, myStat.AP);
                yield return new WaitForSeconds(1.0f);
            }
        }
    }*/

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

    public void AttackT()
    {
        Collider[] list = Physics.OverlapSphere(myAttackPoint.position, 1f, myEnemyMask);
        foreach (Collider col in list)
        {
            col.GetComponent<IBattle>()?.OnDamage(myStat.AP);
        }
    }

    public void ComboCheck(bool v)
    {
        if (v)
        {
            //Start Combo Check
            IsCombable = true;
            clickCount = 0;
        }
        else
        {
            //End Combo Check
            IsCombable = false;
            if (clickCount == 0)
            {
                myAnim.SetTrigger("ComboFail");
            }
        }
    }
    void CheckGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.0f, layer))
        {
            OnGround = true;
        }
        else
        {
            OnGround = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((myEnemyMask & 1 << other.gameObject.layer) != 0)
        {
            myEnemys.Add(other.transform);
            if (_targeting == null)
            {
                _targeting = other.GetComponent<Monster>();
                StartCoroutine(Targetting());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        myEnemys.Remove(other.transform);
        if (_targeting.transform == other.transform)
        {
            FindCloseTarget();
        }
    }

    void FindCloseTarget()
    {
        _targeting = null;
        float min = Mathf.Infinity;
        int sel = 0;
        for (int i = 0; i < myEnemys.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, myEnemys[i].position);
            if (dist < min)
            {
                sel = i;
                min = dist;
            }
        }
        if (myEnemys.Count > 0) _targeting = myEnemys[sel].GetComponent<Monster>();
    }

    IEnumerator Targetting()
    {
        while (_targeting != null)
        {
            Quaternion rot = Quaternion.LookRotation((_targeting.transform.position - transform.position).normalized);
            //transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10.0f);
            yield return null;
        }
    }
}
