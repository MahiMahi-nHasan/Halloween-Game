using UnityEngine;

[System.Serializable]
public class ChaseBehaviour : AIBehaviour
{
    public string targetPropertyName = "_Target";
    public string sightPropertyName = "_SeeingTarget";

    private Vector3 lastKnownPosition;

    public override void Initialize(GameObject parent)
    {
        base.Initialize(parent);
    }

    /*
    Returns the last known position of the target. The last known position is updated if:
     - The NPC can see the target
     - The NPC is following a command
    */
    public override Vector3 SelectTarget()
    {
        Vector3 t_pos = new(
            GameManager.active.player.position.x,
            parent.transform.position.y,
            GameManager.active.player.position.z
        );

        // If the NPC can see the player, using the sight property from the NPC class
        if ((bool)typeof(NPC).GetProperty(sightPropertyName).GetValue(parent.GetComponent<NPC>()))
            lastKnownPosition = t_pos;

        return lastKnownPosition;
    }
}