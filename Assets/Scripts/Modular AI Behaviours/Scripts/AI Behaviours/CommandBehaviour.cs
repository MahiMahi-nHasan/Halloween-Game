using UnityEngine;

[CreateAssetMenu(fileName = "Command Behaviour", menuName = "AI Behaviours/Behaviours/New Command Behaviour")]
public class CommandBehaviour : AIBehaviour
{
    public string targetTag = "Player";

    public new void Initialize(GameObject parent)
    {
        base.Initialize(parent);
    }

    public override Vector3 SelectTarget()
    {
        return GameObject.FindGameObjectWithTag(targetTag).transform.position;
    }
}