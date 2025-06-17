using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text quantityText;

    private ItemData itemData;

    public void SetItem(ItemData data)
    {
        itemData = data;
        icon.sprite = data.icon;
        icon.enabled = true;
        quantityText.text = data.quantity.ToString();
    }

    public void ClearSlot()
    {
        itemData = null;
        icon.sprite = null;
        icon.enabled = false;
        quantityText.text = "";
    }

    public ItemData GetItem() => itemData;
}
