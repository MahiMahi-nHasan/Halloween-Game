using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName="Patrol Behaviour", menuName="AI Behaviours/Behaviours/New Patrol Behaviour")]
public class PatrolBehaviour : AIBehaviour
{
    private Vector3 initialPosition;

    [SerializeField] private float patrolRadius = 10;
    [SerializeField] private bool drawPatrolRadius = false;

    // Call base constructor to set constraints
    public new void Initialize(GameObject parent)
    {
        Debug.Log("Initializing Patrol Behaviour");
        base.Initialize(parent);
        Debug.Log(parent.transform.position);
        initialPosition = parent.transform.position;
    }

    public override Vector3 SelectTarget()
    {
        Vector2 rand = Random.insideUnitCircle * patrolRadius;
        return initialPosition + new Vector3(
            rand.x,
            parent.transform.position.y,
            rand.y
        );
    }

    public new void Gizmos()
    {
        if (drawPatrolRadius)
            Handles.DrawWireDisc(initialPosition, Vector3.up, patrolRadius, 0.5f);
    }
}