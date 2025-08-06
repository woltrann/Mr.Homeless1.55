using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class IAP : MonoBehaviour
{
    public static IAP Instance;
    public float coin;
    public Text coinText;
    public GameObject infoPanel;


    [Header("Ads")]
    public bool isAdsRemoved = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        coin = PlayerPrefs.GetFloat("Coin", 0);
        coinText.text = coin.ToString();

        isAdsRemoved = PlayerPrefs.GetInt("RemoveAds", 0) == 1;
    }


    public void AddCoin(float amount)
    {
        coin += amount;
        coinText.text = coin.ToString();
        PlayerPrefs.SetFloat("Coin", coin);
    }

    public void RemoveCoin(float amount)
    {
        coin -= amount;
        coinText.text = coin.ToString();
        PlayerPrefs.SetFloat("Coin", coin);
    }

    public void AddGold(int amount)
    {
        StatManager.Instance.money += amount;
        StatManager.Instance.moneyText.text = StatManager.Instance.money.ToString();
        PlayerPrefs.SetFloat("Money", StatManager.Instance.money);
        PlayerPrefs.Save();
    }

    public void RemoveAds()
    {
        isAdsRemoved = true;
        PlayerPrefs.SetInt("RemoveAds", 1);
    }

    private IEnumerator panelopen()
    {
        infoPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        infoPanel.SetActive(false);
    }

    public void panelOpen()
    {
        StartCoroutine(panelopen());
    }

    public void BuyCoin20() => IAPManager.Instance.BuyProduct("coin_20");
    public void BuyCoin50() => IAPManager.Instance.BuyProduct("coin_50");
    public void BuyCoin100() => IAPManager.Instance.BuyProduct("coin_100");
    public void BuyCoin250() => IAPManager.Instance.BuyProduct("coin_250");

    public void BuyGold5000() => IAPManager.Instance.BuyProduct("gold_5000");
    public void BuyGold10000() => IAPManager.Instance.BuyProduct("gold_10000");
    public void BuyGold25000() => IAPManager.Instance.BuyProduct("gold_25000");
    public void BuyGold50000() => IAPManager.Instance.BuyProduct("gold_50000");

    public void BuyRemoveAds() => IAPManager.Instance.BuyProduct("remove_ads");

}
