using UnityEngine;

public enum ItemType
{
    Consumable,   // Kullan�labilir (enerji i�ece�i vs.)
    Equipment,    // Ekipman (�apka, ayakkab� vs.)
    QuestItem,    // G�rev i�in �zel item
    Other         // Di�er her �ey
}

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public int price;
    public ItemType itemType;
    public bool stackable = true; // Ayn� item'dan birden fazla olabilir mi?
}
