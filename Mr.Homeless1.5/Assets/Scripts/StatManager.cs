using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatManager : MonoBehaviour
{
    public static StatManager Instance;

    [Header("BasicStats")]
    public int money;
    public int fame;
    private int health = 80;
    private int maxHealth = 100;
    private int energy = 80;
    private int maxEnergy = 100;

    [Header("PowerStats")]
    public int weaponPower;    // Silah gücü
    public int equipmentPower; // Ekipman (şapka, ayakkabı, vs.)
    public int dogPower;       // Köpek gücü
    public int armorPower;     // Çelik yelek
    public int carPower;       // Araba gücü
    public int supportPower;   // Destek gücü (mesela kahve içmekten gelen enerji gibi)
    public int hatPower;

    public int attackPower => Mathf.Clamp(weaponPower + equipmentPower + dogPower, 0, 1000000);
    public int defencePower => Mathf.Clamp( armorPower + carPower, 0, 1000000);
    public int totalPower => Mathf.Clamp(attackPower + defencePower + supportPower , 0, 3000000);

    public Slider healthSlider;
    public Slider energySlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI energyText;

    public TextMeshProUGUI ResourceText ;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI fameText;
    public TextMeshProUGUI weaponPowerText;
    public TextMeshProUGUI equipmentPowerText;
    public TextMeshProUGUI dogPowerText;
    public TextMeshProUGUI armorPowerText;
    public TextMeshProUGUI carPowerText;
    public TextMeshProUGUI hatPowerText;
    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI defencePowerText;
    public TextMeshProUGUI supportPowerText;
    public TextMeshProUGUI totalPowerText;
    

    
    void Awake()
    {
         Instance = this; 
    }
    void Start()
    {
        healthSlider.minValue = 0;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
        healthText.text = health.ToString() + "/" + healthSlider.maxValue.ToString();

        energySlider.minValue = 0;
        energySlider.maxValue = maxEnergy;
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
                    fame = Mathf.Clamp(fame + change.amount, 0, 1000000);
                    fameText.text = fame.ToString();
                    changesSummary += $"Şöhret: {change.amount}\n";
                    break;

                case StatType.Health:
                    health= Mathf.Clamp(health + change.amount, 0, maxHealth);
                    healthSlider.value = health;    // Slider’ı güncelle
                    healthText.text = health.ToString() + "/" + healthSlider.maxValue.ToString();
                    changesSummary += $"Can: {change.amount}\n";
                    break;

                case StatType.Energy:
                    energy = Mathf.Clamp(energy + change.amount, 0, maxEnergy);
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

    public void SetWeaponPower(int value) => weaponPower = value;
    public void SetEquipmentPower(int value) => equipmentPower = value;
    public void SetDogPower(int value) => dogPower = value;
    public void SetArmorPower(int value) => armorPower = value;
    public void SetCarPower(int value) => carPower = value;
    public void SetHatPower(int value) => hatPower = value;
    public void SetSupportPower(int value) => supportPower = value;


    public void ChangeMoney(int value)
    {
        money += value;
        moneyText.text = money.ToString();
    }
    public void AddEnergy(int value)
    {
        energy = Mathf.Clamp(energy + value, 0, maxEnergy);
        energySlider.value = energy;    
        energyText.text = energy.ToString() + "/" + energySlider.maxValue.ToString();
    }
    public void AddHealth(int value)
    {
        health = Mathf.Clamp(health + value, 0, maxHealth);
        healthSlider.value = health;
        healthText.text = health.ToString() + "/" + healthSlider.maxValue.ToString();
    }
}
