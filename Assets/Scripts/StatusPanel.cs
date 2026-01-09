using TMPro;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    [SerializeField] private StatusBar _healthBar;
    [SerializeField] private StatusBar _hungerBar;
    [SerializeField] private StatusBar _thirstBar;
    [SerializeField] private TMP_Text _temperature;

    public void SetUp(PlayerStats playerStats)
    {
        _healthBar.SetUpBar(playerStats.health.currentValue, playerStats.health.maxValue);
        _hungerBar.SetUpBar(playerStats.hunger.currentValue, playerStats.hunger.maxValue);
        _thirstBar.SetUpBar(playerStats.thirst.currentValue, playerStats.thirst.maxValue);
        _temperature.text = $"{playerStats.bodyTemperature}°C";
    }

    public void UpdateHealthBar(float currentHealth, float maxValue)
    {
        _healthBar.SetUpBar(currentHealth, maxValue);
    }

    public void UpdateHungerBar(float currentHunger, float maxValue)
    {
        _hungerBar.SetUpBar(currentHunger, maxValue);
    }

    public void UpdateThirstBar(float currentThirst, float maxValue)
    {
        _thirstBar.SetUpBar(currentThirst, maxValue);
    }

    public void UpdateTemperature(float bodyTemperature)
    {
        _temperature.text = $"{bodyTemperature}°C";
    }
}
