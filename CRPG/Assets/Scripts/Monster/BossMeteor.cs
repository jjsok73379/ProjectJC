using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMeteor : MonoBehaviour
{
    [SerializeField]
    LayerMask enemyMask;
    [SerializeField]
    BossDragon theBossDragon;

    private void Awake()
    {
        SoundManager.Inst.BossSound[0].Play();
        Collider[] myEnemys;
        myEnemys = Physics.OverlapSphere(transform.position, 2, enemyMask);
        foreach (Collider col in myEnemys)
        {
            col.GetComponent<IBattle>()?.OnDamage(theBossDragon.myStat.AP);
        }
    }
}
