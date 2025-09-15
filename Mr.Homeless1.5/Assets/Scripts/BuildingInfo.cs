using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingInfo : MonoBehaviour
{
    public Buildings buildingData;
    public Constructions constructions;

    public void OnTap()
    {
        Debug.Log("Bina t�kland�: " + buildingData.buildingName);

        // MarketUIManager�a bu binan�n datas�n� g�nder
        if (MarketUIManager.Instance != null)
        {
            MarketUIManager.Instance.OpenMarket(buildingData);
        }

        // Building panelini de a�
        BuildingPanelManager.Instance.ShowBuildingInfo(buildingData);
    }
    public void OnTopConstruction()
    {
        Debug.Log("�n��aya t�kland�: " + constructions.buildingName);
        BuildingPanelManager.Instance.ShowConstructionBuildingDetails(constructions);
    }

}
