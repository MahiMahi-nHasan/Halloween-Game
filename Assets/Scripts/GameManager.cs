using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    public MapGen mapGenerator;
    /*
     * for when these things are coded
    public List<EntitySpawner> entitySpawners = new List<EntitySpawner>();
    */

    [Header("Game Data")]
    public int candy = 0;
    void Start()
    {
        if (mapGenerator != null)
        {
            mapGenerator.GenerateMap();
        }

        UIManager.Instance.UpdateCandy(candy);
    }
    void Update()
    {
        
    }
}
