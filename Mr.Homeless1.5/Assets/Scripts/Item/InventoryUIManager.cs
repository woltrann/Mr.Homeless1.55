using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    [Header("UI References")]
    public Transform inventoryGridParent;
    public GameObject inventorySlotPrefab;
    private int columns = 8;
    private int rows = 4;

    [Header("Item Detail UI")]
    public GameObject itemDetailPanel;
    public TextMeshProUGUI itemDetailName;
    public TextMeshProUGUI itemDetailDescription;

    [Header("Character Equipment UI")]
    public Image weaponSlotImage;
    public Image equipmentSlotImage;
    public Image dogSlotImage;
    public Image armorSlotImage;
    public Image carSlotImage;
    public Image hatSlotImage;


    private List<GameObject> slotList = new List<GameObject>();
    private GridLayoutGroup gridLayout;
    public Button useButton;
    public Button equipButton;
    public Image selectedItemIcon;
    private ItemData selectedItem;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gridLayout = inventoryGridParent.GetComponent<GridLayoutGroup>();
    }

    private void Start()
    {
        AdjustGridCellSize(); // <- Ekle buraya
        CreateInventorySlots(columns * rows);
        UpdateInventoryUI();
    }

    void CreateInventorySlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryGridParent);
            slotList.Add(slot);

            int index = i; // closure problemi için index kopyala
            Button btn = slot.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                if (index < InventoryManager.Instance.inventory.Count)
                    OnSlotClicked(InventoryManager.Instance.inventory[index]);
            });
        }
    }

    public void UpdateInventoryUI()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            Image icon = slotList[i].transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI amountText = slotList[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            if (i < InventoryManager.Instance.inventory.Count)
            {
                ItemData item = InventoryManager.Instance.inventory[i];
                icon.sprite = item.itemIcon;
                icon.gameObject.SetActive(true);

                amountText.text = "1"; // Stack mantığı ileride eklenebilir
                amountText.gameObject.SetActive(true);
            }
            else
            {
                icon.gameObject.SetActive(false);
                amountText.gameObject.SetActive(false);
            }
        }

        // 🔥 Kuşanılan itemleri karakter slotlarına yansıt
        if (weaponSlotImage != null)
        {
            if (InventoryManager.Instance.equippedWeapon != null)
                weaponSlotImage.sprite = InventoryManager.Instance.equippedWeapon.itemIcon;
            else
                weaponSlotImage.sprite = null;
        }

        if (equipmentSlotImage != null)
        {
            if (InventoryManager.Instance.equippedEquipment != null)
                equipmentSlotImage.sprite = InventoryManager.Instance.equippedEquipment.itemIcon;
            else
                equipmentSlotImage.sprite = null;
        }

        if (dogSlotImage != null)
        {
            if (InventoryManager.Instance.equippedDog != null)
                dogSlotImage.sprite = InventoryManager.Instance.equippedDog.itemIcon;
            else
                dogSlotImage.sprite = null;
        }

        if (armorSlotImage != null)
        {
            if (InventoryManager.Instance.equippedArmor != null)
                armorSlotImage.sprite = InventoryManager.Instance.equippedArmor.itemIcon;
            else
                armorSlotImage.sprite = null;
        }

        if (carSlotImage != null)
        {
            if (InventoryManager.Instance.equippedCar != null)
                carSlotImage.sprite = InventoryManager.Instance.equippedCar.itemIcon;
            else
                carSlotImage.sprite = null;
        }

        if (hatSlotImage != null)
        {
            if (InventoryManager.Instance.equippedHat != null)
                hatSlotImage.sprite = InventoryManager.Instance.equippedHat.itemIcon;
            else
                hatSlotImage.sprite = null;
        }

    }


    void OnSlotClicked(ItemData item)
    {
        selectedItem = item;

        // Paneli aç
        itemDetailPanel.SetActive(true);
        selectedItemIcon.sprite = item.itemIcon;
        itemDetailName.text = item.itemName;
        itemDetailDescription.text = item.longDescription;

        useButton.gameObject.SetActive(item.itemType == ItemType.Consumable);
        equipButton.gameObject.SetActive(item.itemType == ItemType.Equipment);

        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(() =>
        {
            InventoryManager.Instance.UseItem(item);
            UpdateInventoryUI();
            ClearSelection();
        });

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() =>
        {
            InventoryManager.Instance.EquipItem(item);
            UpdateInventoryUI();
            ClearSelection();
        });
    }

    public void ClearSelection()
    {
        selectedItem = null;
        selectedItemIcon.sprite = null;
        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        itemDetailName.text = " ";
        itemDetailDescription.text = " ";
    }
    private void AdjustGridCellSize()
    {
        if (gridLayout == null) return;

        RectTransform rt = inventoryGridParent.GetComponent<RectTransform>();
        float panelWidth = rt.rect.width - gridLayout.padding.left - gridLayout.padding.right;
        float panelHeight = rt.rect.height - gridLayout.padding.top - gridLayout.padding.bottom;

        float cellWidth = (panelWidth - (gridLayout.spacing.x * (columns - 1))) / columns;
        float cellHeight = cellWidth;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
