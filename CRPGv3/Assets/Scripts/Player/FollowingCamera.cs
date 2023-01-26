using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform myTarget;
    Vector3 myDir = Vector3.zero;
    float targetDist = 0.0f;
    float dist = 0.0f;
    public Vector2 ZoomRange = new Vector2(0.5f, 8.0f);
    public float Height = 1.0f;
    public float ZoomSpeed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        myDir = transform.position - myTarget.position;
        targetDist = dist = myDir.magnitude;
        myDir.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        targetDist += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        targetDist = Mathf.Clamp(targetDist, ZoomRange.x, ZoomRange.y);

        dist = Mathf.Lerp(dist, targetDist, Time.deltaTime * 5.0f);
        transform.position = myTarget.position + myDir * dist + Vector3.up * Height;        
    }
}
