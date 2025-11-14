using Pathfinding;
using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;

// Set up to work with the Unity AstarPathfindingProject by Aron Granberg
[System.Serializable]
public abstract class AIBehaviour
{
    protected GameObject parent;
    public float minimumInterval;
    public float randomInterval;
    public bool continuous = false;
    public float speedModifier = 0;
    public float multiplicativeSpeedMultiplier = 1;
    public virtual void Initialize(GameObject parent)
    {
        this.parent = parent;
    }
    public abstract Vector3 SelectTarget();
    
    public virtual void Gizmos()
    {
        // Can be overwritten in child classes
    }
}

/*
[CustomEditor(typeof(AIBehaviour), true)]
public class AIBehaviourEditor : Editor
{
    SerializedProperty minimumInterval_prop;
    SerializedProperty randomInterval_prop;
    SerializedProperty continuous_prop;
    SerializedProperty speedModifier_prop;
    SerializedProperty multiplicativeSpeedModifier_prop;
    SerializedProperty constraints_prop;

    public void OnEnable()
    {
        minimumInterval_prop = serializedObject.FindProperty("minimumInterval");
        randomInterval_prop = serializedObject.FindProperty("randomInterval");
        continuous_prop = serializedObject.FindProperty("continuous");
        speedModifier_prop = serializedObject.FindProperty("speedModifier");
        multiplicativeSpeedModifier_prop = serializedObject.FindProperty("multiplicativeSpeedMultiplier");
        constraints_prop = serializedObject.FindProperty("constraints");
    }

    public new void OnInspectorGUI()
    {
        PropertyField(minimumInterval_prop);
        PropertyField(randomInterval_prop);
        PropertyField(continuous_prop);
        PropertyField(speedModifier_prop);
        PropertyField(multiplicativeSpeedModifier_prop);
        PropertyField(constraints_prop);

        serializedObject.Update();
    }
}
*/