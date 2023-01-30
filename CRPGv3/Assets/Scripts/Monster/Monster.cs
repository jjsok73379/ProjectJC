using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BattleSystem
{
    public GameObject AIPer = null;
    [field:SerializeField]
    public Transform AttackPoint
    {
        get; 
        private set;
    }
    Vector3 startPos = Vector3.zero;
    public enum STATE
    {
        Create, Idle, Roaming, Battle, Dead
    }
    public STATE myState = STATE.Create;

    void ChangeState(STATE s)
    {
        if(myState == s) return;
        myState = s;
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Idle:
                StartCoroutine(DelayRoaming(2.0f));
                break;
            case STATE.Roaming:
                Vector3 pos = Vector3.zero;
                pos.x = Random.Range(-3.0f, 3.0f);
                pos.z = Random.Range(-3.0f, 3.0f);
                pos = startPos + pos;
                MoveToPosition(pos,() => ChangeState(STATE.Idle));
                break;
            case STATE.Battle:
                AttackTarget(myTarget);
                break;
            case STATE.Dead:
                AIPer.SetActive(false);
                GetComponent<CapsuleCollider>().enabled = false;
                StopAllCoroutines();
                myAnim.SetTrigger("Dead");
                foreach(IBattle ib in myAttackers)
                {
                    ib.DeadMessage(transform);
                }
                StartCoroutine(DisApearing(2.0f, 2.0f));
                break;
        }
    }

    void StateProcess()
    {
        switch (myState)
        {
            case STATE.Create:
                break;
            case STATE.Idle:
                break;
            case STATE.Roaming:
                break;
            case STATE.Battle:
                break;
            case STATE.Dead:
                break;
        }
    }

    IEnumerator DelayRoaming(float t)
    {
        yield return new WaitForSeconds(t);
        ChangeState(STATE.Roaming);
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        ChangeState(STATE.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
    }

    public void FindTarget(Transform target)
    {
        //if (myState == STATE.Dead) return;
        myTarget = target;
        StopAllCoroutines();
        ChangeState(STATE.Battle);
    }

    public void LostTarget()
    {
        //if (myState == STATE.Dead) return;
        myTarget = null;
        StopAllCoroutines();
        myAnim.SetBool("IsMoving", false);
        myAnim.SetBool("BattleMode", false);
        ChangeState(STATE.Idle);
    }

    public override void OnDamage(float dmg)
    {
        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(-3.0f, 3.0f);
        pos.z = Random.Range(-3.0f, 3.0f);
        myStat.HP -= dmg;
        if (Mathf.Approximately(myStat.HP, 0.0f))
        {
            ChangeState(STATE.Dead);
            ObjectManager.Inst.DropItemToPosition(transform.position - pos, ObjectManager.Inst.BookPrefab, ObjectManager.Inst.books, 0, 31);
            ObjectManager.Inst.DropItemToPosition(transform.position, ObjectManager.Inst.PotionPrefab, ObjectManager.Inst.potions, 28, 100);
        }
        else
        {
            myAnim.SetTrigger("Damage");
        }
    }
    public override bool IsLive()
    {
        return myState != STATE.Dead;
    }
    public override void DeadMessage(Transform tr)
    {
        if (tr == myTarget)
        {
            LostTarget();
        }
    }

    IEnumerator DisApearing(float d, float t)
    {
        yield return new WaitForSeconds(t);
        float dist = d;
        while (dist > 0.0f)
        {
            float delta = 0.5f * Time.deltaTime;
            if (delta > dist)
            {
                delta = dist;
            }
            dist -= delta;
            transform.Translate(Vector3.down * delta, Space.World);
            yield return null;
        }
        Destroy(gameObject);
    }
}
