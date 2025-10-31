using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    [SerializeField] bool freezeXZAxis = true;
    [SerializeField] bool bobbing = false;
    [SerializeField] float bobAmplitude = 0.1f;
    [SerializeField] float bobFrequency = 1f;
    Vector3 initialPos;
    private void Start()
    {
        initialPos = transform.localPosition;
    }
    void LateUpdate()
    {
        if (freezeXZAxis)
            transform.rotation = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
        else
            transform.rotation = Camera.main.transform.rotation;
        if (bobbing)
        {
            float bobOffset = Mathf.Sin(Time.time * bobFrequency) * bobAmplitude;
            transform.localPosition = initialPos + new Vector3(0f, bobOffset, 0f);
        }
    }
}
