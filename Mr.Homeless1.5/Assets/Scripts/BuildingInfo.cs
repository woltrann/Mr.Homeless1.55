using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingInfo : MonoBehaviour
{
    public Buildings buildingData;
    public Constructions constructions;

    public void OnTap()
    {
        Debug.Log("Bina týklandý: " + buildingData.buildingName);

        // MarketUIManager’a bu binanýn datasýný gönder
        if (MarketUIManager.Instance != null)
        {
            MarketUIManager.Instance.OpenMarket(buildingData);
        }

        // Building panelini de aç
        BuildingPanelManager.Instance.ShowBuildingInfo(buildingData);
    }
    public void OnTopConstruction()
    {
        Debug.Log("Ýnþþaya týklandý: " + constructions.buildingName);
        BuildingPanelManager.Instance.ShowConstructionBuildingDetails(constructions);
    }

}
