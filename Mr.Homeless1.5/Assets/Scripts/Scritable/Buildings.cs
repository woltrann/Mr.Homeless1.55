using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingAction
{
    public string buttonText;
    public Sprite icon;
    public ButtonType actionType;
    public float cooldownDuration = 5f; // 🔥 Eklenen kısım
}


public enum ButtonType
{
    OpenMarket,
    StartJob,
    TalkToNPC,
    UpgradeBuilding
}

[CreateAssetMenu(fileName = "Buildings", menuName = "Scriptable Objects/Buildings")]
public class Buildings : ScriptableObject
{
    public string buildingName;
    [TextArea] public string description;
    public Sprite buildingImage;
    public List<BuildingAction> actions;
}
