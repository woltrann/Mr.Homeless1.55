using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemData> inventory = new List<ItemData>();

    [Header("References")]
    public ItemDatabase itemDatabase; // 🔥 Inspector’dan bağla

    // Kuşanılan itemlar
    public ItemData equippedWeapon;
    public ItemData equippedEquipment;
    public ItemData equippedDog;
    public ItemData equippedArmor;
    public ItemData equippedCar;
    public ItemData equippedHat;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemData item)
    {
        int maxSlots = 32;
        if (inventory.Count >= maxSlots)
        {
            Debug.Log("Envanter dolu! Eklenemedi: " + item.itemName);
            return;
        }

        inventory.Add(item);
        SaveInventory();
        InventoryUIManager.Instance.UpdateInventoryUI();
    }

    public void SaveInventory()
    {
        InventorySave saveData = new InventorySave(inventory, equippedWeapon, equippedEquipment, equippedDog, equippedArmor, equippedCar, equippedHat);
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("PlayerInventory", json);
    }

    public void LoadInventory()
    {
        if (!PlayerPrefs.HasKey("PlayerInventory")) return;

        string json = PlayerPrefs.GetString("PlayerInventory");
        InventorySave saveData = JsonUtility.FromJson<InventorySave>(json);

        inventory.Clear();

        foreach (var savedItem in saveData.items)
        {
            ItemData realItem = itemDatabase.GetItemByName(savedItem.itemName);
            if (realItem != null)
                inventory.Add(realItem);
        }

        equippedWeapon = inventory.Find(i => i != null && i.itemName == saveData.equippedWeaponID);
        equippedEquipment = inventory.Find(i => i != null && i.itemName == saveData.equippedEquipmentID);
        equippedDog = inventory.Find(i => i != null && i.itemName == saveData.equippedDogID);
        equippedArmor = inventory.Find(i => i != null && i.itemName == saveData.equippedArmorID);
        equippedCar = inventory.Find(i => i != null && i.itemName == saveData.equippedCarID);
        equippedHat = inventory.Find(i => i != null && i.itemName == saveData.equippedHatID);

        InventoryUIManager.Instance.UpdateInventoryUI();
    }

    public void UseItem(ItemData item)
    {
        if (item.itemType == ItemType.Consumable)
        {
            switch (item.itemName.ToLower())
            {
                case "kahve":
                    StatManager.Instance.AddEnergy(item.powerAmount);
                    break;
                case "ilaç":
                    StatManager.Instance.AddHealth(item.powerAmount);
                    break;
                default:
                    Debug.Log("Bilinmeyen consumable: " + item.itemName);
                    break;
            }
            inventory.Remove(item);
        }
        else if (item.itemType == ItemType.Equipment)
        {
            EquipItem(item);
        }
        else if (item.itemType == ItemType.QuestItem)
        {
            Debug.Log(item.itemName + " sadece satılabilir.");
        }

        InventoryUIManager.Instance.UpdateInventoryUI();
        SaveInventory();
    }

    public void EquipItem(ItemData item)
    {
        if (item.itemType != ItemType.Equipment) return;

        switch (item.equipmentType)
        {
            case EquipmentType.Weapon:
                equippedWeapon = item;
                StatManager.Instance.SetWeaponPower(item.powerAmount);
                StatManager.Instance.weaponPowerText.text = "Silah: " + item.itemName;
                break;
            case EquipmentType.Equipment:
                equippedEquipment = item;
                StatManager.Instance.SetEquipmentPower(item.powerAmount);
                StatManager.Instance.equipmentPowerText.text = "Ekipman: " + item.itemName;
                break;
            case EquipmentType.Dog:
                equippedDog = item;
                StatManager.Instance.SetDogPower(item.powerAmount);
                StatManager.Instance.dogPowerText.text = "Köpek: " + item.itemName;
                break;
            case EquipmentType.Armor:
                equippedArmor = item;
                StatManager.Instance.SetArmorPower(item.powerAmount);
                StatManager.Instance.armorPowerText.text = "Çelik Yelek: " + item.itemName;
                break;
            case EquipmentType.Car:
                equippedCar = item;
                StatManager.Instance.SetCarPower(item.powerAmount);
                StatManager.Instance.carPowerText.text = "Araba: " + item.itemName;
                break;
            case EquipmentType.Hat:
                equippedHat = item;
                StatManager.Instance.SetHatPower(item.powerAmount);
                StatManager.Instance.hatPowerText.text = "Şapka: " + item.itemName;
                break;
        }

        StatManager.Instance.attackPowerText.text = "Saldırı Gücü: " + StatManager.Instance.attackPower;
        StatManager.Instance.defencePowerText.text = "Savunma Gücü: " + StatManager.Instance.defencePower;
        StatManager.Instance.totalPowerText.text = "Toplam Güç: " + StatManager.Instance.totalPower;
        StatManager.Instance.supportPowerText.text = "Destek Gücü: " + StatManager.Instance.supportPower;

        Debug.Log(item.itemName + " kuşanıldı!");
        SaveInventory();
        InventoryUIManager.Instance.UpdateInventoryUI();
    }

    [System.Serializable]
    public class InventorySave
    {
        public List<ItemSaveData> items;

        public string equippedWeaponID;
        public string equippedEquipmentID;
        public string equippedDogID;
        public string equippedArmorID;
        public string equippedCarID;
        public string equippedHatID;

        public InventorySave(List<ItemData> items, ItemData weapon, ItemData equipment, ItemData dog, ItemData armor, ItemData car, ItemData hat)
        {
            this.items = new List<ItemSaveData>();
            foreach (var i in items)
                this.items.Add(new ItemSaveData(i));

            equippedWeaponID = weapon != null ? weapon.itemName : "";
            equippedEquipmentID = equipment != null ? equipment.itemName : "";
            equippedDogID = dog != null ? dog.itemName : "";
            equippedArmorID = armor != null ? armor.itemName : "";
            equippedCarID = car != null ? car.itemName : "";
            equippedHatID = hat != null ? hat.itemName : "";
        }
    }
}
