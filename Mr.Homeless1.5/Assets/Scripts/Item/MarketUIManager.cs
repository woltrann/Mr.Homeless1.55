using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MarketUIManager : MonoBehaviour
{
    public GameObject marketPanel;
    public Transform contentParent; // ScrollView > Content
    public GameObject itemPrefab;
    public List<Item> itemsForSale; // Inspector’dan doldur


    private void OnEnable()
    {
        UpdateMarket();
    }

    public void UpdateMarket()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (Item item in itemsForSale)
        {
            GameObject itemGO = Instantiate(itemPrefab, contentParent);
            itemGO.transform.Find("ItemName").GetComponent<Text>().text = item.itemName;
            itemGO.transform.Find("ItemPrice").GetComponent<Text>().text = "$" + item.price;
            itemGO.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.icon;

            Button buyButton = itemGO.transform.Find("BuyButton").GetComponent<Button>();
            buyButton.onClick.AddListener(() => BuyItem(item));
        }

    }

    public void BuyItem(Item item)
    {
        if (StatManager.Instance.money >= item.price)
        {
            StatManager.Instance.money -= item.price;
            StatManager.Instance.moneyText.text = StatManager.Instance.money.ToString();
            PlayerInventory.Instance.AddItem(item);
            UpdateMarket();
        }
        else
        {
            Debug.Log("Yetersiz bakiye.");
        }
    }


    public void ToggleMarketPanel(bool show)
    {
        marketPanel.SetActive(show);
        if (show) UpdateMarket();
    }
}
