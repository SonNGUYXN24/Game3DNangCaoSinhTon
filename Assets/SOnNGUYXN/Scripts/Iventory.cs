using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>();

    public void AddItem(ItemData.ItemType type, int amount = 1)
    {
        ItemData existingItem = items.Find(i => i.itemType == type);
        if (existingItem != null)
        {
            existingItem.quantity += amount;
        }
        else
        {
            items.Add(new ItemData { itemType = type, quantity = amount });
        }
    }
}
