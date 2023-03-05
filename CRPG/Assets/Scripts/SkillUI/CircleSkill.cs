using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSkill : MonoBehaviour
{
    void Update()
    {
        FollowMouse();
    }

    void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            transform.position = new Vector3(hit.point.x, transform.position.y + 1, hit.point.z);
        }
    }
}
