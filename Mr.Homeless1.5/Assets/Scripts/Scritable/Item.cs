using UnityEngine;

public enum ItemType
{
    Consumable,   // Kullanýlabilir (enerji içeceði vs.)
    Equipment,    // Ekipman (þapka, ayakkabý vs.)
    QuestItem,    // Görev için özel item
    Other         // Diðer her þey
}

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public int price;
    public ItemType itemType;
    public bool stackable = true; // Ayný item'dan birden fazla olabilir mi?
}
