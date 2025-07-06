using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;

    public int money;
    public int fame;
    public int hunger;
    public int energy;

    public Slider hungerSlider;
    public Slider energySlider;
    public Text hungr;
    public Text energ;


    public Text moneyText;
    public TextMeshProUGUI ResourceText ;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        hungerSlider.minValue = 0;
        hungerSlider.maxValue = 100;
        hungerSlider.value = hunger;

        energySlider.minValue = 0;
        energySlider.maxValue = 100;
        energySlider.value = energy;
    }
    public void ApplyStatChanges(List<StatChange> changes)
    {
        string changesSummary = ""; // 🪐 Değişim özetini tutacak

        foreach (var change in changes)
        {
            switch (change.statType)
            {
                case StatType.Money:
                    money += change.amount;
                    moneyText.text = money.ToString();

                    changesSummary += $"<sprite name=\"can\"> Para: {change.amount}$\n";
                    break;
                case StatType.Fame:
                    fame += change.amount;
                    changesSummary += $"Şöhret: {change.amount}\n";
                    break;
                case StatType.Hunger:
                    hunger += change.amount;
                    changesSummary += $"Açlık: {change.amount}\n";
                    hungerSlider.value = hunger;    // Slider’ı güncelle
                    hungr.text = hunger.ToString()+"/"+hungerSlider.maxValue.ToString();
                    break;
                case StatType.Energy:
                    energy += change.amount;
                    changesSummary += $"Enerji: {change.amount}\n";
                    energySlider.value = energy;    // Slider’ı güncelle
                    energ.text = energy.ToString() + "/" + energySlider.maxValue.ToString();
                    break;
            }
        }
        ResourceText.text = changesSummary;
    }
}
