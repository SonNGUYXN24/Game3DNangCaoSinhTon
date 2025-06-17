using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Raycast : MonoBehaviour
{
    [SerializeField] private List<LayerMask> layerMasks; // 0: Grass, 1: Rocks, 2: Trees
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private GameObject pickupPrompt;

    private Transform currentTarget;
    private int currentLayerIndex = -1;
    private Inventory inventory;

    [System.Obsolete]
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Inventory not found in scene.");
        }
        statusPlayer = FindObjectOfType<StartusPlayer>();
    }

    void Update()
    {
        bool found = false;
        currentTarget = null;
        currentLayerIndex = -1;

        for (int i = 0; i < layerMasks.Count; i++)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayDistance, layerMasks[i]))
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
                currentTarget = hit.transform;
                currentLayerIndex = i;
                found = true;
                break;
            }
        }

        pickupPrompt.SetActive(found);

        if (found && Input.GetKeyDown(KeyCode.E) && currentTarget != null)
        {
            CollectItem(currentLayerIndex);
            Destroy(currentTarget.gameObject);
            currentTarget = null;
        }

        if (!found)
        {
            Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.blue);
        }
    }

    private void CollectItem(int layerIndex)
    {
        if (inventory == null || statusPlayer == null) return;

        switch (layerIndex)
        {
            case 0:
                inventory.AddItem(ItemData.ItemType.Grass);
                statusPlayer.GainExp(200);
                break;
            case 1:
                inventory.AddItem(ItemData.ItemType.Rock);
                statusPlayer.GainExp(500);
                break;
            case 2:
                inventory.AddItem(ItemData.ItemType.Tree);
                statusPlayer.GainExp(300);
                break;
        }
    }
    private StartusPlayer statusPlayer;
}
