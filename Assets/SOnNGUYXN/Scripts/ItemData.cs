using UnityEngine;

[System.Serializable]
public class ItemData
{
    public enum ItemType
    {
        Grass,
        Rock,
        Wood,  // ✅ Thêm dòng này nếu chưa có
        Arrow 
    }

    public ItemType itemType;
    public int quantity;
    public Sprite icon;
}
