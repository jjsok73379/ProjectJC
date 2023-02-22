using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonTargetingRegion : MonoBehaviour
{
    [SerializeField]
    Camera Cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseCursorPos();
    }

    public void MouseCursorPos()
    {
        Ray ray = Cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mouseDir = new Vector3(hit.point.x, 0, hit.point.z);
            transform.position = mouseDir;
        }
    }
}
