using UnityEngine;

public class Cashier : NPC
{
    public void Awake() {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    public void Update() {
        /*
        Check if NPC can see target
        Returns true if the raycast hits no obstacles (returns false), false if the raycast 
        hits obstacles (returns true)
         > ie seeingTarget = !Raycast
        */
        float dist;
        eyePos = transform.position + Vector3.up * height;
        targetAtEyeHeight = target.position + Vector3.up * height;
        seeingTarget = !Physics.Raycast(
            eyePos,
            targetAtEyeHeight - eyePos,
            seeingDistance > (dist = Vector3.Distance(eyePos, targetAtEyeHeight)) ? dist : seeingDistance,
            obstacleLayer
        );

        if (seeingTarget && sawCandyStolen) {
            Debug.Log("Cashier saw player stealing!");
            GameObject[] fcs = GameObject.FindGameObjectsWithTag("FloorClerk");
            fcs[Random.Range(0, fcs.Length)].GetComponent<FloorClerk>().ReceiveCommand();
        }
    }
}