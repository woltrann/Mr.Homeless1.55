using UnityEngine;

public enum ItemType
{
    Consumable,  // Kahve, yiyecek, enerji veren itemler
    Equipment,   // Silah, şapka, ayakkabı, kuşanılabilir itemler
    QuestItem,   // Sadece satılabilir veya görevle ilgili (mücevher gibi)
}
public enum EquipmentType
{
    Weapon,
    Equipment,
    Dog,
    Armor,
    Car,
    Hat,
    Support    
}


[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/Item")]
public class ItemData : ScriptableObject
{
     


    public string itemName;
    [TextArea] public string shortDescription;
    [TextArea] public string longDescription;
    public Sprite itemIcon;
    public int price;

    public ItemType itemType;               // 🔥 yeni alan
    public EquipmentType equipmentType;     // Sadece itemType == Equipment ise geçerli
    public int powerAmount;                 // Silah vs. güç veya kahve vs. enerji
}
