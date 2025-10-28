using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class Stat
{
    public float maxValue;
    public float currentValue;

    public Stat(float curr, float max)
    {
        currentValue = curr;
        maxValue = max;
    }

    public void Add(float amount)
    {
        currentValue += amount;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
    }

    public void Subtract(float amount)
    {
        currentValue -= amount;
        if (currentValue < 0)
        {
            currentValue = 0;
        }
    }

    public void SetToMax()
    {
        currentValue = maxValue;
    }
}

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private SO_PlayerStats _playerStats;
    [SerializeField] private float _regenStaminaRate = 2f;
    [SerializeField] private float _regenStaminaDelay = 1f;

    public bool isDead = false;
    public bool isHungry = false;
    public bool isThirsty = false;
    public bool isExhausted = false;
    public bool isTemperatureCritical = false;

    private float _lastStaminaUseTime;

    private void Update()
    {
        if (Time.time - _lastStaminaUseTime >= _regenStaminaDelay)
        {
            _playerStats.stamina.Add(_regenStaminaRate * Time.deltaTime);
            if (_playerStats.stamina.currentValue >= _playerStats.stamina.maxValue)
            {
                isExhausted = false;
            }
        }
    }

    public void UseStamina(float amount)
    {
        _playerStats.stamina.Subtract(amount);
        if (_playerStats.stamina.currentValue <= 0)
        {
            isExhausted = true;
        }

        _lastStaminaUseTime = Time.time;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        float damageTaken = damage;
        _playerStats.health.Subtract(damageTaken);
        if (_playerStats.health.currentValue <= 0 && !isDead)
        {
            isDead = true;
        }
    }

    public void CalculateDamage(ref float damage)
    {

    }

    public void ApplyDamage(float damage)
    {
        TakeDamage(damage);
    }

    public void CheckState()
    {

    }
}
