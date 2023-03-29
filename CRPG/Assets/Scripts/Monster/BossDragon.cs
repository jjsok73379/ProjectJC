using CombineRPG;
using DTT.AreaOfEffectRegions;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossDragon : Monster
{
    [SerializeField]
    ParticleSystem myBreath;
    public Slider myHPSlider;
    [SerializeField]
    TMP_Text HPText;
    [SerializeField]
    BossZone myZone;
    [SerializeField]
    GameObject[] Regions;
    [SerializeField]
    Transform HitPoint;
    [SerializeField]
    LayerMask enemyMask;
    [SerializeField]
    Vector3[] MeteorPos;
    [SerializeField]
    ParticleSystem BossClear;
    float orgDamage;
    Collider[] myEnemys;
    int ranAction;

    // Start is called before the first frame update
    void Start()
    {
        myBreath.gameObject.SetActive(false);
        myHPSlider.gameObject.SetActive(false);
        myHPSlider.value = 1;
        myStat.changeHp = (float v) => myHPSlider.value = v;
        for (int i = 0; i < Regions.Length; i++)
        {
            Regions[i].gameObject.SetActive(false);
        }
        BossClear.gameObject.SetActive(false);
        orgDamage = myStat.AP;
    }

    // Update is called once per frame
    void Update()
    {
        HPText.text = myStat.maxHp + " / " + myStat.HP;
        if (myBreath.gameObject.activeSelf)
        {
            if (myAnim.GetBool("IsFlame"))
            {
                myBreath.Play();
            }
            else
            {
                myBreath.Stop();
            }
        }
        if (myZone.IsEnter)
        {
            StartCoroutine(CheckEnter());
        }
        if (myState == STATE.Dead)
        {
            StartCoroutine(CheckBossDead());
        }
        if (myState == STATE.Battle)
        {
            transform.LookAt(myTarget.transform);
        }
    }

    IEnumerator CheckEnter()
    {
        yield return new WaitForSeconds(13.0f);
        myHPSlider.gameObject.SetActive(true);
        myZone.IsEnter = false;
    }

    IEnumerator CheckBossDead()
    {
        StopAllCoroutines(); 
        for (int i = 0; i < Regions.Length; i++)
        {
            Regions[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(3.0f);
        BossClear.gameObject.SetActive(true);
        BossClear.Play();
    }

    public IEnumerator Think()
    {
        StopAllCoroutines();
        yield return new WaitForSeconds(0.1f);

        if (myStat.HP > 70000)
        {
            ranAction = Random.Range(0, 2);
        }
        else if (myStat.HP < 70000 && myStat.HP > 30000)
        {
            ranAction = Random.Range(1, 4);
        }
        else if (myStat.HP < 30000)
        {
            ranAction = Random.Range(2, 5);
        }
        switch (ranAction)
        {
            case 0:
                StartCoroutine(BasicAttack());
                break;
            case 1:
                // 발톱
                StartCoroutine(ClawAttack());
                break;
            case 2:
                // 브레스
                StartCoroutine(FlameAttack());
                break;
            case 3:
                // 메테오
                StartCoroutine(MeteorAttack());
                break;
            case 4:
                // 날아서 브레스
                StartCoroutine(FlyingBreath());
                break;
        }
    }

    IEnumerator BasicAttack()
    {
        AttackTarget(myTarget, myAttackSound);
        yield return new WaitForSeconds(3.0f);

        StartCoroutine(Think());
    }

    IEnumerator ClawAttack()
    {
        Regions[0].SetActive(true);
        Regions[0].transform.position = transform.position + new Vector3(5.0f, 1.0f, -5.0f);
        while (Regions[0].GetComponent<LineRegion>().FillProgress <= 0.99f)
        {
            yield return null;
            Regions[0].GetComponent<LineRegion>().FillProgress += 0.4f * Time.deltaTime;
        }
        myStat.AP = 500f;
        Regions[0].SetActive(false);
        Regions[0].GetComponent<LineRegion>().FillProgress = 0;
        myAnim.SetTrigger("ClawAttack");
        yield return new WaitForSeconds(5.0f);
        myStat.AP = orgDamage;

        StartCoroutine(Think());
    }

    public void ClawAttackTarget()
    {
        SoundManager.Inst.BossSound[2].Play();
        Collider[] list = Physics.OverlapSphere(HitPoint.position, 2, enemyMask);

        if (list.Length > 0)
        {
            foreach (Collider col in list)
            {
                col.GetComponent<IBattle>()?.OnDamage(myStat.AP);
            }
        }
    }

    IEnumerator FlameAttack()
    {
        Regions[1].SetActive(true);
        Regions[1].transform.position = transform.position + new Vector3(5.0f, 1.0f, -5.0f);
        while (Regions[1].GetComponent<ArcRegion>().FillProgress <= 0.99)
        {
            yield return null;
            Regions[1].GetComponent<ArcRegion>().FillProgress += 0.3f * Time.deltaTime;
        }
        myStat.AP = 400f;
        Regions[1].SetActive(false);
        Regions[1].GetComponent<ArcRegion>().FillProgress = 0;
        myBreath.gameObject.SetActive(true);
        UseFlame();
        myAnim.SetTrigger("FlameAttack");
        yield return new WaitForSeconds(6.0f);
        myStat.AP = orgDamage;

        StartCoroutine(Think());
    }

    IEnumerator MeteorAttack()
    {
        myStat.AP = 350f;
        myAnim.SetTrigger("MeteorAttack");
        int ranNum = Random.Range(0, MeteorPos.Length);
        for (int i = 2; i < Regions.Length; i++)
        {
            Regions[i].SetActive(true);
            Regions[i].transform.position = transform.position + MeteorPos[ranNum];
            while (Regions[i].GetComponent<CircleRegion>().FillProgress <= 0.99)
            {
                yield return null;
                Regions[i].GetComponent<CircleRegion>().FillProgress += 0.1f * Time.deltaTime;
                if (Regions[i].GetComponent<CircleRegion>().FillProgress > 0.9)
                {
                    SoundManager.Inst.BossSound[0].Play();
                    Regions[i].GetComponentInChildren<ParticleSystem>().Play();
                }
            }
            myEnemys = Physics.OverlapSphere(transform.position, 2, enemyMask);
            foreach (Collider col in myEnemys)
            {
                col.GetComponent<IBattle>()?.OnDamage(myStat.AP);
            }
            Regions[i].GetComponent<CircleRegion>().FillProgress = 0;
            Regions[i].SetActive(false);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(5.0f);
        myStat.AP = orgDamage;

        StartCoroutine(Think());
    }

    IEnumerator FlyingBreath()
    {
        myAnim.SetTrigger("FlyFlame");
        myStat.AP = 800f;
        yield return new WaitForSeconds(5.0f);
        myBreath.gameObject.SetActive(true);
        UseFlame();
        yield return new WaitForSeconds(11.0f);
        myStat.AP = orgDamage;

        StartCoroutine(Think());
    }

    public void UseFlame()
    {
        SoundManager.Inst.BossSound[1].Play();
        myEnemys = Physics.OverlapSphere(transform.position, 18.0f, enemyMask);
        float RadianRange = Mathf.Cos((90.0f / 2) * Mathf.Deg2Rad);

        for (int i = 0; i < myEnemys.Length; i++)
        {
            float targetRadian = Vector3.Dot(transform.forward, (myEnemys[i].transform.position - transform.position).normalized);
            if (targetRadian > RadianRange)
            {
                if (myEnemys[i].gameObject.layer == 8)
                {
                    myEnemys[i].gameObject.GetComponent<IBattle>()?.OnDamage(myStat.AP);
                }
            }
        }
    }
}
