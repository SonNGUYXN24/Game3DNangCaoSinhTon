using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Raycast : MonoBehaviour
{
    [SerializeField] private List<LayerMask> layerMasks; // 0: Grass, 1: Rocks, 2: Trees
    [SerializeField] private float rayDistance = 5f;

    [Header("UI Elements")]
    [SerializeField] private GameObject pickupPrompt;
    [SerializeField] public TextMeshProUGUI grassCountText;
    public TextMeshProUGUI rocksCountText;
    public TextMeshProUGUI treesCountText;
    [SerializeField] private GameObject crosshair; // hình ảnh hồng tâm, đặt giữa màn hình

    [Header("Inventory System")]
    [SerializeField] private Inventory inventory;

    private Transform currentTarget;
    private int currentLayerIndex = -1;

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

        pickupPrompt?.SetActive(found);
        crosshair?.SetActive(true); // luôn bật crosshair

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
                inventory.AddItem(ItemData.ItemType.Grass, 1);
                grassCountText.text = "Grass: " + GetItemQuantity(ItemData.ItemType.Grass);
                break;

            case 1: // Rocks
                inventory.AddItem(ItemData.ItemType.Rock, 1);
                rocksCountText.text = "Rocks: " + GetItemQuantity(ItemData.ItemType.Rock);
                break;

            case 2: // Trees
                inventory.AddItem(ItemData.ItemType.Wood, 1);
                treesCountText.text = "Trees: " + GetItemQuantity(ItemData.ItemType.Wood);
                break;
        }
    }

    private int GetItemQuantity(ItemData.ItemType type)
    {
        var item = inventory.items.Find(i => i.itemType == type);
        return item != null ? item.quantity : 0;
    }
    public void UpdateRockUI()
    {
        var item = inventory.items.Find(i => i.itemType == ItemData.ItemType.Rock);
        int quantity = item != null ? item.quantity : 0;
        rocksCountText.text = "Rocks: " + quantity;
    }
    public void UpdateWoodUI()
{
    var item = inventory.items.Find(i => i.itemType == ItemData.ItemType.Wood);
    int quantity = item != null ? item.quantity : 0;
    treesCountText.text = "Trees: " + quantity;
}

}
