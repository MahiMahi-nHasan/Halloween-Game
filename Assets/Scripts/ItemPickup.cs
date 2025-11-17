using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int quantity = 1;
    public AudioClip pickupSound;

    private bool playerInRange = false;
    private GameManager gameManager;

    private InputActions input;
    public ItemSpawnpoint parent;

    private void Awake()
    {
        input = new();

        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    void Update()
    {
        if (playerInRange && input.Player.Pickup.WasPressedThisFrame())
            PickupItem();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void PickupItem()
    {
        // Candy stolen logic
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 30f); // 30 units radius
        foreach (var col in hitColliders)
        {
            if (!col.TryGetComponent(out NPC npc)) continue;

            if (!Physics.Raycast(transform.position, col.transform.position - transform.position, Vector3.Distance(transform.position, col.transform.position), GameManager.active.obstacleLayers))
            {
                Debug.Log("NPC saw candy stolen");
                npc.sawCandyStolen = true;
            }
        }

        // Destroy or disable the item
        gameManager.candy += quantity;
        GameManager.active.universalSoundEffect.PlayOneShot(pickupSound);
        UIManager.Instance.UpdateCandy(gameManager.candy);
        parent.spawned = false;

        Debug.Log("Candy stolen!");

        Destroy(gameObject);
    }
}
