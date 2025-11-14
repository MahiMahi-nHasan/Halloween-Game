using System.Reflection;
using UnityEngine;

public class Trigger
{
    private NPC parent;

    public enum Type
    {
        BOOLEAN,
        FLOAT,
        METHOD
    }
    public Type type;
    public string propertyName;

    public bool setBooleanValue;

    public float setFloatValue;

    public Constraint[] constraints;

    public void Initialize(NPC parent)
    {
        //Debug.Log("Initializing trigger");
        this.parent = parent;
    }

    public void Invoke()
    {
        switch (type)
        {
            case Type.BOOLEAN:
                typeof(NPC).GetProperty(propertyName).SetValue(parent, setBooleanValue);
                break;
            case Type.FLOAT:
                typeof(NPC).GetProperty(propertyName).SetValue(parent, setFloatValue);
                break;
            case Type.METHOD:
                Debug.Log("Triggering");
                typeof(NPC).InvokeMember(propertyName, BindingFlags.Public, null, parent, null);
                break;
        }
    }
}