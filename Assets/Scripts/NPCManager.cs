using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<NPC> NPCsInTrigger = new List<NPC>();
    public void AddEnemy(NPC target)
    {
        NPCsInTrigger.Add(target);
    }
    public void RemoveEnemy(NPC target)
    {
        NPCsInTrigger.Remove(target);
    }
}
