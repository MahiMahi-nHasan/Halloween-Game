using UnityEngine;

[System.Serializable]
public class CommandBehaviour : AIBehaviour
{
    public override Vector3 SelectTarget()
    {
        return GameManager.active.player.position;
    }
}