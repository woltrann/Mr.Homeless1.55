using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingInfo : MonoBehaviour
{
    public Buildings buildingData;

    public void OnTap()
    {
        Debug.Log("Bina týklandý: " + buildingData.buildingName);
        BuildingPanelManager.Instance.ShowBuildingInfo(buildingData);
    }
}
