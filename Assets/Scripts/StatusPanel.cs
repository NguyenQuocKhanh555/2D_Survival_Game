using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    [SerializeField] private StatusBar _healthBar;
    [SerializeField] private StatusBar _hungerBar;
    [SerializeField] private StatusBar _thirstBar;

    public void SetUp(PlayerStats playerStats)
    {
        _healthBar.SetUpBar(playerStats.health.currentValue, playerStats.health.maxValue);
        _hungerBar.SetUpBar(playerStats.hunger.currentValue, playerStats.hunger.maxValue);
        _thirstBar.SetUpBar(playerStats.thirst.currentValue, playerStats.thirst.maxValue);
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
}
