using CombineRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
//public delegate void MyAction();

public class CharacterMovement : CharacterProperty
{
    Coroutine moveCo = null;
    Coroutine rotCo = null;
    Coroutine comboattackCo = null;
    protected void AttackTarget(Transform target, AudioSource audio)
    {
        StopAllCoroutines();
        comboattackCo = StartCoroutine(AttckingTarget(target, myStat.AttackRange, myStat.AttackDelay, audio));
    }

    protected void MoveToPosition(Vector3 pos, UnityAction done = null, bool Rot = true)
    {
        if (comboattackCo != null)
        {
            StopCoroutine(comboattackCo);
            comboattackCo = null;
        }
        if (moveCo != null)
        {
            StopCoroutine(moveCo);
            moveCo = null;
        }
        moveCo = StartCoroutine(MovingToPostion(pos,done));

        if (Rot)
        {
            if (rotCo != null)
            {
                StopCoroutine(rotCo);
                rotCo = null;
            }
            rotCo = StartCoroutine(RotatingToPosition(pos));
        }
    }

    IEnumerator RotatingToPosition(Vector3 pos)
    {
        Vector3 dir = (pos - transform.position).normalized;
        float Angle = Vector3.Angle(transform.forward, dir);
        float rotDir = 1.0f;
        if (Vector3.Dot(transform.right, dir) < 0.0f)
        {
            rotDir = -rotDir;
        }

        while (Angle > 0.0f)
        {
            float delta = myStat.RotSpeed * Time.deltaTime;
            if (delta > Angle)
            {
                delta = Angle;
            }
            Angle -= delta;
            transform.Rotate(Vector3.up * rotDir * delta, Space.World);

            yield return null;
        }
    }


    IEnumerator MovingToPostion(Vector3 pos, UnityAction done)
    {
        Vector3 dir = pos - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();

        //달리기 시작
        myAnim.SetBool("IsMoving", true);
        while (dist > 0.0f)
        {
            float delta = myStat.MoveSpeed * Time.deltaTime;
            if (delta > dist)
            {
                delta = dist;
            }
            dist -= delta;
            transform.Translate(dir * delta, Space.World);
            yield return null;
        }
        //달리기 끝 - 도착
        myAnim.SetBool("IsMoving", false);
        done?.Invoke();
    }

    IEnumerator AttckingTarget(Transform target, float AttackRange, float AttackDelay, AudioSource audio)
    {
        float playTime = 0.0f;
        float delta = 0.0f;
        while (target != null)
        {
            playTime += Time.deltaTime;
            //이동
            Vector3 dir = target.position - transform.position;
            float dist = dir.magnitude;
            dir.Normalize();
            if (dist > AttackRange)
            {
                myAnim.SetBool("IsMoving", true);
                delta = myStat.MoveSpeed * Time.deltaTime;
                if (delta > dist)
                {
                    delta = dist;
                }
                transform.Translate(dir * delta, Space.World);
            }     
            else
            {
                myAnim.SetBool("IsMoving", false);
                if(playTime >= AttackDelay)
                {
                    //공격
                    playTime = 0.0f;
                    audio.Play();
                    myAnim.SetTrigger("Attack");
                }
            }
            //회전
            delta = myStat.RotSpeed * Time.deltaTime;
            float Angle = Vector3.Angle(dir, transform.forward);
            float rotDir = 1.0f;
            if(Vector3.Dot(transform.right, dir) < 0.0f)
            {
                rotDir = -rotDir;
            }
            if(delta > Angle)
            {
                delta = Angle;
            }
            transform.Rotate(Vector3.up * delta * rotDir, Space.World);
            yield return null;
        }
        myAnim.SetBool("IsMoving", false);
    }
}
