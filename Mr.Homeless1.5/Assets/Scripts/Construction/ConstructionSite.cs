using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ConstructionState { Locked, Purchased, Built }

public class ConstructionSite : MonoBehaviour
{
    private ConstructionSite currentSite;
    public ConstructionState state = ConstructionState.Locked;
    public GameObject currentBuilding; // Kurulan bina prefab��
    public GameObject wireNet;
    public GameObject ground;
    public TextMeshProUGUI infoConstruction;
    public int price; // Sat�n alma fiyat�

    // Panel referanslar�
    private Button buyButton;


    void Start()
    {
        if (BuildingPanelManager.Instance.buyPanel != null)
            buyButton = BuildingPanelManager.Instance.buyPanel.GetComponentInChildren<Button>();
    }


    public void OnClick()
    {
        currentSite = this; // �u an t�klanan in�aat alan�n� kaydet
        BuildingPanelManager.Instance.currentConstructionSite = this;
        infoConstruction.text = "Bu in�aat alan�n� " + price +"$ kar��l���nda sat�n almak ister misiniz?";
        switch (state)
        {
            case ConstructionState.Locked:
                BuildingPanelManager.Instance.buyPanel.SetActive(true); // �Sat�n almak ister misin?�
                break;

            case ConstructionState.Purchased:
                BuildingPanelManager.Instance.chooseBuildingPanel.SetActive(true); // �Hangi bina kurulacak?�
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
            Debug.Log($"{name} sat�n al�nd�!");
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
            Debug.Log($"{buildingPrefab.name} kuruldu {name} alan�na!");
        }
    }

}
