using CombineRPG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Debuff
{
    public enum Type
    {
        Slow, Stun
    }
    public Type type;
    public float value;
    public float keepTime;
}

public class Monster : BattleSystem
{
    public List<Debuff> debuffList = new List<Debuff>();
    int rewardMoney; 
    [SerializeField]
    int MinMoney; 
    [SerializeField]
    int MaxMoney; 
    [SerializeField]
    GameObject SwordPrefab;
    [SerializeField]
    GameObject HP_PotionPrefab;
    [SerializeField]
    GameObject MP_PotionPrefab;
    [SerializeField]
    GameObject BookPrefab;
    [SerializeField]
    GameObject myMinimapIcon;
    public int GiveExp;
    public Transform myHeadTop;
    Transform Player;
    HpBar myUI = null;
    float SlowMoveSpeed = 0.5f;
    float StunMoveSpeed = 0.0f;
    Color orgColor = Color.white;

    public GameObject AIPer = null;
    [field:SerializeField]
    public Transform AttackPoint
    {
        get; 
        private set;
    }
    Vector3 startPos = Vector3.zero;
    Vector3[] DropPos = new Vector3[4];
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
                pos.x = UnityEngine.Random.Range(-3.0f, 3.0f);
                pos.z = UnityEngine.Random.Range(-3.0f, 3.0f);
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
                foreach (IBattle ib in myAttackers)
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
        rewardMoney = UnityEngine.Random.Range(MinMoney, MaxMoney);
        GameObject obj = Instantiate(Resources.Load("Prefabs/HpBar"), GameManager.Inst.HpBars) as GameObject;
        myUI = obj.GetComponent<HpBar>();
        myUI.myTarget = myHeadTop;
        myStat.changeHp = (float v) => myUI.myBar.value = v;

        orgColor = GetComponentInChildren<Renderer>().material.color;

        startPos = transform.position;
        startPos.y = 1.63f;
        
        for (int i = 0; i < DropPos.Length; i++)
        {
            DropPos[i].x = UnityEngine.Random.Range(-3.0f, 3.0f);
            DropPos[i].z = UnityEngine.Random.Range(-3.0f, 3.0f);
        }
        ChangeState(STATE.Idle);
    }

    // Update is called once per frame
    void Update()
    {
        StateProcess();
        for(int i = 0; i < debuffList.Count;)
        {
            Debuff temp = debuffList[i];
            temp.keepTime -= Time.deltaTime;
            if (temp.keepTime <= 0.0f)
            {
                switch(temp.type)
                {
                    case Debuff.Type.Slow:
                        SlowMoveSpeed /= temp.value;
                        break;
                    case Debuff.Type.Stun:
                        StunMoveSpeed /= temp.value;
                        break;
                }
                debuffList.RemoveAt(i);
                continue;
            }
            debuffList[i] = temp;
            ++i;
        }
    }

    public void AddDebuff(Debuff.Type type, float value, float keep)
    {
        for (int i = 0; i < debuffList.Count; i++)
        {
            if (type == debuffList[i].type)
            {
                Debuff temp = debuffList[i];
                temp.keepTime = keep;
                debuffList[i] = temp;
                return;
            }
        }

        Debuff def = new Debuff();
        def.type = type;
        def.value = value;
        def.keepTime = keep;

        switch (type)
        {
            case Debuff.Type.Slow:
                StartCoroutine(DamagingColor(Color.blue, keep));
                SlowMoveSpeed *= value;
                break;
            case Debuff.Type.Stun:
                StartCoroutine(DamagingColor(Color.yellow, keep));
                myAnim.SetTrigger("Stun");
                StunMoveSpeed *= value;
                break;
        }
        debuffList.Add(def);
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
        Debug.Log("À¸¾Ç");
        if (Mathf.Approximately(myStat.HP, 0.0f))
        {
            ChangeState(STATE.Dead);
            ObjectManager.Inst.DropCoinToPosition(transform.position, rewardMoney);
            DropItem(new Vector3(transform.position.x + DropPos[1].x, 2.5f, transform.position.z + DropPos[1].z), 0, 22, SwordPrefab);
            DropItem(new Vector3(transform.position.x + DropPos[2].x, 2.5f, transform.position.z + DropPos[2].z), 15, 90, HP_PotionPrefab);
            DropItem(new Vector3(transform.position.x + DropPos[3].x, 2.5f, transform.position.z + DropPos[3].z), 15, 90, MP_PotionPrefab);
            DropItem(new Vector3(transform.position.x + DropPos[0].x, 2.0f, transform.position.z + DropPos[0].z), 0, 25, BookPrefab);
        }
        else
        {
            myAnim.SetTrigger("Damage");
            StartCoroutine(SearchWide(2.0f));
        }
    }

    IEnumerator SearchWide(float t)
    {
        AIPer.GetComponent<SphereCollider>().radius *= 3;
        yield return new WaitForSeconds(t);
        AIPer.GetComponent<SphereCollider>().radius = 7;
    }

    IEnumerator DamagingColor(Color color, float t)
    {
        GetComponentInChildren<Renderer>().material.color = color;
        yield return new WaitForSeconds(t);
        GetComponentInChildren<Renderer>().material.color = orgColor;
    }

    void DropItem(Vector3 pos, int ranmin, int ranmax, GameObject itemprefab)
    {
        int RandomDrop;
        RandomDrop = UnityEngine.Random.Range(ranmin, ranmax);
        if (RandomDrop > 20)
        {
            Instantiate(itemprefab, pos, Quaternion.identity);
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
        Destroy(myMinimapIcon);
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
