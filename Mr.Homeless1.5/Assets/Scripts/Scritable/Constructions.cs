using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Constructions", menuName = "Scriptable Objects/Constructions")]
public class Constructions : ScriptableObject
{
    [Header("Genel Bilgiler")]
    public string buildingName;
    public int cost;
    [TextArea] public string buyDescription;
    [TextArea] public string infoDescription;
    public Sprite buildingIcon;
    public GameObject prefab; // Bu binanýn prefab referansý

    [Header("Kazanç")]
    public int incomePerHour;
    public int incomePerDay;

    [Header("Upgrade")]
    public int upgradeCost;
    public Constructions nextLevel; // Ýleride yükseltme için
}
