using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;

    public int money;
    public int fame;
    public int power;
    public int health;
    public int energy;

    public Slider healthSlider;
    public Slider energySlider;
    public Text healthText;
    public Text energyText;

    public Text moneyText;
    public Text fameText;
    public TextMeshProUGUI ResourceText ;
    void Awake()
    {
         Instance = this; 
    }

    void Start()
    {
        healthSlider.minValue = 0;
        healthSlider.maxValue = 100;
        healthSlider.value = health;
        healthText.text = health.ToString() + "/" + healthSlider.maxValue.ToString();

        energySlider.minValue = 0;
        energySlider.maxValue = 100;
        energySlider.value = energy;
        energyText.text = energy.ToString() + "/" + energySlider.maxValue.ToString();

        moneyText.text = money.ToString();
        fameText.text = fame.ToString();

    }
    public void ApplyStatChanges(List<StatChange> changes)
    {
        string changesSummary = ""; //  Değişim özetini tutacak
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
                    fameText.text = fame.ToString();
                    changesSummary += $"Şöhret: {change.amount}\n";
                    break;

                case StatType.Health:
                    health += change.amount;
                    healthSlider.value = health;    // Slider’ı güncelle
                    healthText.text = health.ToString() + "/" + healthSlider.maxValue.ToString();
                    changesSummary += $"Can: {change.amount}\n";
                    break;

                case StatType.Energy:
                    energy += change.amount;
                    energySlider.value = energy;    // Slider’ı güncelle
                    energyText.text = energy.ToString() + "/" + energySlider.maxValue.ToString();
                    changesSummary += $"Enerji: {change.amount}\n";
                    break;
            }
        }
        ResourceText.text = changesSummary;
        HealthEnergyController();
    }
    public void HealthEnergyController()
    {
        if (healthSlider.value <= 0 || energySlider.value <= 0)
        {
            health=0;
            energy=0;
            healthText.text = health.ToString() + "/" + healthSlider.maxValue.ToString();
            energyText.text = energy.ToString() + "/" + energySlider.maxValue.ToString();
            Congrats.Instance.GameOver();
        }
    }
}
