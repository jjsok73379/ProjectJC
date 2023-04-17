using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour
{
    public UnityEvent<Transform> FindTarget = default;
    public UnityEvent LostTarget = default;
    public LayerMask enemyMask = default;
    public Transform myTarget = null;

    private void OnTriggerEnter(Collider other)
    {
        if (myTarget != null) return;
        if ((enemyMask & 1 << other.gameObject.layer) != 0)
        {
            //타겟을 처음 발견했을때
            if (other.transform.GetComponent<IBattle>().IsLive())
            {
                GetComponentInParent<Animator>().SetBool("BattleMode", true);
                myTarget = other.transform;
                FindTarget?.Invoke(myTarget);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (myTarget == other.transform)
        {
            myTarget = null;
            LostTarget?.Invoke();
        }
    }
}
