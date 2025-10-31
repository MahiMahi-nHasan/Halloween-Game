using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Shoot : MonoBehaviour
{
    [SerializeField] public float range = 20;
    [SerializeField] public float verticalRange = 20;
    public NPCManager enemyManager;
    public GameManager gameManager;
    public float fireRate = 1.5f;
    public LayerMask raycastLayerMask;

    private BoxCollider shootTrigger;
    private float cooldown;

    void Start()
    {
        shootTrigger = GetComponent<BoxCollider>();
        shootTrigger.size = new Vector3(1, verticalRange, range);
        shootTrigger.center = new Vector3(0, 0, range * 0.5f);
    }
    void Update()
    {
        if (cooldown > 0f)
            cooldown -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && gameManager.candy > 0 && cooldown <= 0f) 
        {
            ThrowCandy();
            cooldown = fireRate;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        NPC target = other.GetComponent<NPC>();
        if (target != null && target.tag.Equals("Hostile")) 
        {
            enemyManager.AddEnemy(target);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        NPC target = other.GetComponent<NPC>();
        if (target != null)
        {
                enemyManager.RemoveEnemy(target);
        }

    }
    private void ThrowCandy()
    {
        foreach (var NPC in enemyManager.NPCsInTrigger)
        {
            var dir = NPC.transform.position - transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, range * 1.5f, raycastLayerMask))
                if (hit.transform == NPC.transform)
                    NPC.takeDamage(25f);
        }
        gameManager.candy--;
        UIManager.Instance.UpdateCandy(gameManager.candy);

    }

}
