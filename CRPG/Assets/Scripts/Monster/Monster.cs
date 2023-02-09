using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : BattleSystem
{
    int rewardMoney; 
    [SerializeField]
    int MinMoney; 
    [SerializeField]
    int MaxMoney; 
    [SerializeField]
    GameObject SwordPrefab;
    public int GiveExp;
    public Transform myHeadTop;
    HpBar myUI = null;

    public GameObject AIPer = null;
    [field:SerializeField]
    public Transform AttackPoint
    {
        get; 
        private set;
    }
    Vector3 startPos = Vector3.zero;
    Vector3[] DropPos = new Vector3[3];

    MinimapIcon myIcon = null;
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
        rewardMoney = Random.Range(MinMoney, MaxMoney);
        GameObject obj = Instantiate(Resources.Load("Prefabs/HpBar"), GameManager.Inst.HpBars) as GameObject;
        myUI = obj.GetComponent<HpBar>();
        myUI.myTarget = myHeadTop;
        myStat.changeHp = (float v) => myUI.myBar.value = v;

        obj = Instantiate(Resources.Load("Prefabs/MinimapIcon"), GameManager.Inst.Minimap) as GameObject;
        myIcon = obj.GetComponent<MinimapIcon>();
        myIcon.Initialize(transform, Color.red);

        startPos = transform.position;
        startPos.y = 1.63f;
        
        for (int i = 0; i < DropPos.Length; i++)
        {
            DropPos[i].x = Random.Range(-3.0f, 3.0f);
            DropPos[i].z = Random.Range(-3.0f, 3.0f);
        }
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
        myStat.HP -= dmg;
        if (Mathf.Approximately(myStat.HP, 0.0f))
        {
            ChangeState(STATE.Dead);
            ObjectManager.Inst.DropItemToPosition(transform.position, ObjectManager.Inst.BookPrefab, ObjectManager.Inst.books, 0, 31, 0);
            ObjectManager.Inst.DropItemToPosition(transform.position + DropPos[0], ObjectManager.Inst.PotionPrefab, ObjectManager.Inst.potions, 28, 100, 0);
            ObjectManager.Inst.DropItemToPosition(transform.position + DropPos[1], ObjectManager.Inst.CoinPrefab, ObjectManager.Inst.coins, 21, 90, rewardMoney);
            DropWeapon(transform.position + DropPos[2]);
        }
        else
        {
            myAnim.SetTrigger("Damage");
        }
    }

    void DropWeapon(Vector3 pos)
    {
        int RandomDrop;
        RandomDrop = Random.Range(0, 22);
        if (RandomDrop > 20)
        {
            Instantiate(SwordPrefab, pos, Quaternion.identity);
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
        Destroy(myUI.gameObject);
        Destroy(myIcon.gameObject);
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
