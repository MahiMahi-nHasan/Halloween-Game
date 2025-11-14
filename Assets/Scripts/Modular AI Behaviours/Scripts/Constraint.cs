using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Constraint", menuName = "AI Behaviours/New Constraint")]
public class Constraint : ScriptableObject
{
    public enum Type
    {
        DISTANCE,
        FLOAT,
        BOOLEAN
    }
    public enum ComparisonType
    {
        EQUAL,
        NOT_EQUAL,
        GREATER_THAN,
        LESS_THAN
    }
    public enum BooleanEvaluationType
    {
        TRUE,
        FALSE
    }

    public Type type;

    public GameObject parent;

    [Tooltip("The type of comparison")]
    public ComparisonType comparisonType;
    public BooleanEvaluationType boolEvaluationType;

    public string propertyName;

    [Tooltip("Value with which to compare the provided property")]
    public float compareValue;

    public static bool Evaluate(IEnumerable<Constraint> list, bool localDebug = false)
    {
        foreach (Constraint c in list)
            if (!c.Evaluate(localDebug))
                return false;

        return true;
    }

    public void Initialize(GameObject parent)
    {
        this.parent = parent;
    }

    public bool Evaluate(bool localDebug = false)
    {
        // Get property from parent
        if (parent == null)
            return false;
        System.Reflection.PropertyInfo property = typeof(NPC).GetProperty(propertyName);;
        NPC parentNPC = parent.GetComponent<NPC>();
        if (parent == null)
            return false;

        void PreMessage(string type)
        {
            if (localDebug)
                Debug.Log(string.Format("{2} - Evaluating {0} as {1}", propertyName, type, parent.name));
        }

        void EvalMessage(bool result)
        {
            if (localDebug)
                Debug.Log(string.Format("{0} evaluated as {1}", name, result));
        }

        bool result;
        switch (type)
        {
            case Type.DISTANCE:
                PreMessage("DISTANCE");
                Transform target = (Transform)property.GetValue(parentNPC);
                float distance = Vector3.Distance(
                    parent.transform.position,
                    target.transform.position
                );
                result = Compare(distance, compareValue);
                EvalMessage(result);
                return result;
            case Type.FLOAT:
                PreMessage("FLOAT");
                result = Compare((float)property.GetValue(parentNPC), compareValue);
                EvalMessage(result);
                return result;
            case Type.BOOLEAN:
                PreMessage("BOOLEAN");
                result = boolEvaluationType == BooleanEvaluationType.TRUE == (bool)property.GetValue(parentNPC);
                EvalMessage(result);
                return result;
            default:
                return false;
        }
    }

    // Custom method to compare two IComparable objects based on the ComparisonType
    public bool Compare<T>(T a, T b) where T : IComparable<T>
    {
        switch (comparisonType)
        {
            case ComparisonType.EQUAL:
                return a.CompareTo(b) == 0;
            case ComparisonType.NOT_EQUAL:
                return a.CompareTo(b) != 0;
            case ComparisonType.GREATER_THAN:
                return a.CompareTo(b) > 0;
            case ComparisonType.LESS_THAN:
                return a.CompareTo(b) < 0;
            default:
                return false;
        }
    }
}