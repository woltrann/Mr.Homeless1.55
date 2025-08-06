using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour, IDetailedStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider extensionProvider;

    public static IAPManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Bu önemli!
        }
        Instance = this;
    }


    void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        builder.AddProduct("coin_20", ProductType.Consumable);
        builder.AddProduct("coin_50", ProductType.Consumable);
        builder.AddProduct("coin_100", ProductType.Consumable);
        builder.AddProduct("coin_250", ProductType.Consumable);
        builder.AddProduct("gold_5000", ProductType.Consumable);
        builder.AddProduct("gold_10000", ProductType.Consumable);
        builder.AddProduct("gold_25000", ProductType.Consumable);
        builder.AddProduct("gold_50000", ProductType.Consumable);
        builder.AddProduct("remove_ads", ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void BuyProduct(string productId)
    {
        if (storeController != null && storeController.products.WithID(productId) != null)
        {
            storeController.InitiatePurchase(productId);
        }
        else
        {
            Debug.LogError("Ürün bulunamadý veya IAP baþlatýlmadý.");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        extensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"IAP baþlatýlamadý: {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log($"Satýn alým baþarýlý: {args.purchasedProduct.definition.id}");

        switch (args.purchasedProduct.definition.id)
        {
            case "coin_20":
                IAP.Instance.AddCoin(20);
                break;
            case "coin_50":
                IAP.Instance.AddCoin(50);
                break;
            case "coin_100":
                IAP.Instance.AddCoin(100);
                break;
            case "coin_250":
                IAP.Instance.AddCoin(250);
                break;
            case "gold_5000":
                IAP.Instance.AddGold(5000);
                break;
            case "gold_10000":
                IAP.Instance.AddGold(10000);
                break;
            case "gold_25000":
                IAP.Instance.AddGold(25000);
                break;
            case "gold_50000":
                IAP.Instance.AddGold(50000);
                break;
            case "remove_ads":
                IAP.Instance.RemoveAds();
                break;
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Satýn alma baþarýsýz: {failureReason}");
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"IAP baþlatýlamadý: {error} - {message}");
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        Debug.LogError($"[DETAYLI] Satýn alma baþarýsýz: {product.definition.id} - {failureDescription.reason} - {failureDescription.message}");
    }

}
