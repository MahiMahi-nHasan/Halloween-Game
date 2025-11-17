using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Tile
{
    public GameObject prefab;
    public bool[] constraints;

    public Tile(GameObject prefab, bool[] constraints)
    {
        this.prefab = prefab;
        this.constraints = constraints;
    }
}
