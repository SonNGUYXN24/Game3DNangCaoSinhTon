using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Danh sách các vật phẩm người chơi hiện có
    public List<ItemData> items = new List<ItemData>();

    /// <summary>
    /// Thêm vật phẩm vào Inventory (từ cả nhặt hoặc chặt đều như nhau)
    /// </summary>
    /// <param name="type">Loại vật phẩm</param>
    /// <param name="amount">Số lượng muốn thêm</param>
    public void AddItem(ItemData.ItemType type, int amount = 1)
    {
        ItemData existingItem = items.Find(i => i.itemType == type);

        if (existingItem != null)
        {
            existingItem.quantity += amount;
        }
        else
        {
            items.Add(new ItemData
            {
                itemType = type,
                quantity = amount
            });
        }
    }

    /// <summary>
    /// Trả về số lượng hiện tại của một loại vật phẩm
    /// </summary>
    public int GetItemQuantity(ItemData.ItemType type)
    {
        ItemData item = items.Find(i => i.itemType == type);
        return item != null ? item.quantity : 0;
    }

    /// <summary>
    /// Tiêu hao vật phẩm (nếu đủ số lượng)
    /// </summary>
    public bool RemoveItem(ItemData.ItemType type, int amount = 1)
    {
        ItemData item = items.Find(i => i.itemType == type);

        if (item != null && item.quantity >= amount)
        {
            item.quantity -= amount;

            if (item.quantity == 0)
                items.Remove(item);

            return true;
        }

        return false;
    }
}
