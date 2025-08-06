using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    public List<Item> ownedItems = new List<Item>();

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(Item item)
    {
        ownedItems.Add(item);
        Debug.Log(item.itemName + " eklendi.");
    }
}
