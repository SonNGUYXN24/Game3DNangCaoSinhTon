using UnityEngine;

[System.Serializable]
public class ItemData
{
    public enum ItemType { Grass, Rock, Tree }

    public ItemType itemType;
    public Sprite icon;
    public int quantity;
}
