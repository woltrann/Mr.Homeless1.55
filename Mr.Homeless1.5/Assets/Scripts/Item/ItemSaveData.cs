using System;

[Serializable]
public class ItemSaveData
{
    public string itemName;
    public ItemType itemType;
    public EquipmentType equipmentType;
    public int powerAmount;
    public string iconPath; // E�er sprite�� da kaydetmek istersen

    public ItemSaveData(ItemData item)
    {
        itemName = item.itemName;
        itemType = item.itemType;
        equipmentType = item.equipmentType;
        powerAmount = item.powerAmount;

        // Sprite yolunu kaydet (sprite'�n Resources klas�r�nde olmas� laz�m)
        if (item.itemIcon != null)
            iconPath = "Icons/" + item.itemIcon.name;
    }
}
