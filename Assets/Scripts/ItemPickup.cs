using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool playerInRange = false;
    private GameManager GameManager;
    private void Start()
    {
        GameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }

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
        Debug.Log("Candy stolen!");

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20f); // 20 units radius
        foreach (var hit in hitColliders)
        {
            NPC npc = hit.GetComponent<NPC>();
            if (npc != null)
            {
                npc._SawCandyStolen = true;
            }
        }

        // Destroy or disable the item
        GameManager.candy++;
        UIManager.Instance.UpdateCandy(GameManager.candy);
        Destroy(gameObject);
    }



}
