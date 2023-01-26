using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject player; // 바라볼 플레이어 오브젝트입니다. 
    public float xmove = 2; // X축 누적 이동량 
    public float ymove = 50; // Y축 누적 이동량 
    public float distance = 20;
    private float wheelspeed = 10.0f;
    private Vector3 Player_Height;
    private Vector3 Player_Side;
    void Start()
    {
        Player_Height = new Vector3(0, 1.5f, 0f);
        Player_Side = new Vector3(-0.8f, 0f, 0f);
    }

    // Update is called once per frame 
    void Update()
    {
        // 마우스 우클릭 중에만 카메라 무빙 적용 
        if (Input.GetMouseButton(1))
        {
            xmove += Input.GetAxis("Mouse X");
            // 마우스의 좌우 이동량을 xmove 에 누적합니다. 
            ymove -= Input.GetAxis("Mouse Y");
            // 마우스의 상하 이동량을 ymove 에 누적합니다. 
        }
        transform.rotation = Quaternion.Euler(ymove, xmove, 0); // 이동량에 따라 카메라의 바라보는 방향을 조정합니다. 
        distance -= Input.GetAxis("Mouse ScrollWheel") * wheelspeed;
        if (distance < 1f) distance = 1f;
        if (distance > 100.0f) distance = 100.0f;
        Vector3 Eye = player.transform.position
            + transform.rotation * Player_Side + Player_Height;
        Vector3 reverseDistance = new Vector3(0.0f, 0.0f, distance);
        // 카메라가 바라보는 앞방향은 Z 축입니다. 이동량에 따른 Z 축방향의 벡터를 구합니다. 
        transform.position = Eye - transform.rotation * reverseDistance;
    }
}