using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float camPan;
    void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        camPan += mouseX; 
        Quaternion lookRot = Quaternion.Euler(0f, camPan, 0f);
        transform.SetPositionAndRotation(transform.position, lookRot);
    }

}
