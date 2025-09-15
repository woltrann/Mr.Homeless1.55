using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class MarketUIManager : MonoBehaviour
{
    public static MarketUIManager Instance;
    [Header("Market Data")]
    public Buildings currentMarket;

    [Header("UI References")]
    public Transform itemListParent; // ScrollView Content
    public GameObject itemButtonPrefab; // Prefab with 2 Text child
    public Image itemDetailIcon;
    public TextMeshProUGUI itemDetailName;
    public TextMeshProUGUI itemDetailDescription;
    public TextMeshProUGUI itemDetailPrice;
    public Button buyButton;
    public Sprite nullSprite;
    private ItemData selectedItem;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        InventoryManager.Instance.LoadInventory();

    }
    public void OpenMarket(Buildings marketData)
    {
        currentMarket = marketData;
        //gameObject.SetActive(true);
        RefreshMarketUI();
    }

    void RefreshMarketUI()
    {
        // Önce eski butonları temizle
        foreach (Transform child in itemListParent)
        {
            Destroy(child.gameObject);
        }

        // Market itemlerini listele
        foreach (var item in currentMarket.purchesableItems)
        {
            GameObject buttonObj = Instantiate(itemButtonPrefab, itemListParent);
            TextMeshProUGUI[] texts = buttonObj.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = item.itemName;
            texts[1].text = item.shortDescription;
            Image[] images = buttonObj.GetComponentsInChildren<Image>();
            images[1].sprite = item.itemIcon;


            Button btn = buttonObj.GetComponent<Button>();
            btn.onClick.AddListener(() => ShowItemDetails(item));
        }
    }

    void ShowItemDetails(ItemData item)
    {
        selectedItem = item;
        itemDetailIcon.sprite = item.itemIcon;
        itemDetailName.text = item.itemName;
        itemDetailDescription.text = item.longDescription;
        itemDetailPrice.text = "Fiyat: " + item.price + "₺";

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => BuySelectedItem(item.price));
    }
    public void ResetSelectedItem()
    {
        selectedItem = null;
        itemDetailIcon.sprite = nullSprite;
        itemDetailName.text = "";
        itemDetailDescription.text = "";
        itemDetailPrice.text = "";
        buyButton.onClick.RemoveAllListeners();
    }
    void BuySelectedItem(int price)
    {
        if (selectedItem != null)
        {
            if (StatManager.Instance.money >= price)
            {
                StatManager.Instance.money -= price;
                StatManager.Instance.moneyText.text = StatManager.Instance.money.ToString();

                InventoryManager.Instance.AddItem(selectedItem);
                InventoryUIManager.Instance.UpdateInventoryUI();
            }
            else
            {
                Debug.Log(selectedItem.itemName + " için para yetersiz!");
            }
        }
    }

}
