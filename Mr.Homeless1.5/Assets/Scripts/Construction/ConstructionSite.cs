using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ConstructionState { Locked, Purchased, Built }

public class ConstructionSite : MonoBehaviour
{
    private ConstructionSite currentSite;
    public ConstructionState state = ConstructionState.Locked;
    public GameObject currentBuilding; // Kurulan bina prefab’ý
    public GameObject wireNet;
    public GameObject ground;
    public TextMeshProUGUI infoConstruction;
    public int price; // Satýn alma fiyatý

    // Panel referanslarý
    private Button buyButton;


    void Start()
    {
        if (BuildingPanelManager.Instance.buyPanel != null)
            buyButton = BuildingPanelManager.Instance.buyPanel.GetComponentInChildren<Button>();
    }


    public void OnClick()
    {
        currentSite = this; // Þu an týklanan inþaat alanýný kaydet
        BuildingPanelManager.Instance.currentConstructionSite = this;
        infoConstruction.text = "Bu inþaat alanýný " + price +"$ karþýlýðýnda satýn almak ister misiniz?";
        switch (state)
        {
            case ConstructionState.Locked:
                BuildingPanelManager.Instance.buyPanel.SetActive(true); // “Satýn almak ister misin?”
                break;

            case ConstructionState.Purchased:
                BuildingPanelManager.Instance.chooseBuildingPanel.SetActive(true); // “Hangi bina kurulacak?”
                break;

            case ConstructionState.Built:
                if (currentBuilding != null)
                {
                    BuildingInfo info = currentBuilding.GetComponent<BuildingInfo>();
                    if (info != null)
                        BuildingPanelManager.Instance.ShowBuildingInfo(info.buildingData);
                }
                break;
        }
    }

    public void BuyLand()
    {
        if (price<=StatManager.Instance.money)
        {
            StatManager.Instance.ChangeMoney(-price);
            state = ConstructionState.Purchased;
            BuildingPanelManager.Instance.buyPanel.SetActive(false);
            Debug.Log($"{name} satýn alýndý!");
            wireNet.SetActive(false);
        }
        else
        {
            BuildingPanelManager.Instance.infoPanel.SetActive(true);
        }
    }
    public void Build(GameObject buildingPrefab)
    {
        if (state == ConstructionState.Purchased)
        {
            currentBuilding = Instantiate(buildingPrefab, transform.position, buildingPrefab.transform.rotation);
            state = ConstructionState.Built;
            BuildingPanelManager.Instance.chooseBuildingPanel.SetActive(false);
            ground.SetActive(false);
            Debug.Log($"{buildingPrefab.name} kuruldu {name} alanýna!");
        }
    }

}
