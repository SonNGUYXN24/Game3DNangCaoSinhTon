using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Raycast : MonoBehaviour
{
    [SerializeField] private List<LayerMask> layerMasks; // 0: Grass, 1: Rocks, 2: Trees
    [SerializeField] private float rayDistance = 5f;

    [Header("UI Elements")]
    [SerializeField] private GameObject pickupPrompt; // TextMeshPro Object "Nhấn E để nhặt"
    [SerializeField] private TextMeshProUGUI grassCountText;
    [SerializeField] private TextMeshProUGUI rocksCountText;
    [SerializeField] private TextMeshProUGUI treesCountText;

    private int grassCount = 0;
    private int rocksCount = 0;
    private int treesCount = 0;

    private Transform currentTarget;
    private int currentLayerIndex = -1;

    void Update()
    {
        bool found = false;
        currentTarget = null;
        currentLayerIndex = -1;

        // Check raycast for each layer
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

        // Handle UI display
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
        switch (layerIndex)
        {
            case 0: // Grass
                grassCount++;
                grassCountText.text = "Grass: " + grassCount;
                break;
            case 1: // Rocks
                rocksCount++;
                rocksCountText.text = "Rocks: " + rocksCount;
                break;
            case 2: // Trees
                treesCount++;
                treesCountText.text = "Trees: " + treesCount;
                break;
        }
    }
}
