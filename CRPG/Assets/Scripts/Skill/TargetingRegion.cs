using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingRegion : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Camera Cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookMouseCursor();
    }

    public void LookMouseCursor()
    {
        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 mouseDir = new Vector3(hit.point.x, transform.rotation.y, hit.point.z) - transform.position;
            animator.transform.forward = mouseDir;
        }
    }
}
