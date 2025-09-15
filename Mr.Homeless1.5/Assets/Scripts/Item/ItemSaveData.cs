using System;

[Serializable]
public class ItemSaveData
{
    public string itemName;
    public ItemType itemType;
    public EquipmentType equipmentType;
    public int powerAmount;
    public string iconPath; // Eðer sprite’ý da kaydetmek istersen

    public ItemSaveData(ItemData item)
    {
        itemName = item.itemName;
        itemType = item.itemType;
        equipmentType = item.equipmentType;
        powerAmount = item.powerAmount;

        // Sprite yolunu kaydet (sprite'ýn Resources klasöründe olmasý lazým)
        if (item.itemIcon != null)
            iconPath = "Icons/" + item.itemIcon.name;
    }
}
