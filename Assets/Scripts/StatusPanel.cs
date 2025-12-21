using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    [SerializeField] private StatusBar _healthBar;
    [SerializeField] private StatusBar _hungerBar;
    [SerializeField] private StatusBar _thirstBar;

    public void SetUp(float currentHealth, float maxHealth, float currentHunger, float maxHunger, float currentThrist, float maxThrist)
    {
        _healthBar.SetUpBar(currentHealth, maxHealth);
        _hungerBar.SetUpBar(currentHunger, maxHunger);
        _thirstBar.SetUpBar(currentThrist, maxThrist);
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
