using UnityEngine;

[CreateAssetMenu(fileName = "Command Behaviour", menuName = "AI Behaviours/Behaviours/New Command Behaviour")]
public class CommandBehaviour : AIBehaviour
{
    public override Vector3 SelectTarget()
    {
        return GameManager.active.player.position;
    }
}