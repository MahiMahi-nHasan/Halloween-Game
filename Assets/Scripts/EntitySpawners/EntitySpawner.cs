using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class EntitySpawner : MonoBehaviour
{
    public Vector3[] locations;
    protected Dictionary<int, GameObject> spawnedEntities = new Dictionary<int, GameObject>();
    public Constraint sleepConstraint;
    public bool spawnOnStart;
    public bool continuous;
    public abstract void TrySpawn();
}
