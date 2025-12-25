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

[Serializable]
public class PlayerStats
{
    public Stat health;
    public Stat stamina;
    public Stat hunger;
    public Stat thirst;
    public float bodyTemperature;
    public int armor;
    
    public PlayerStats(float healthMax, float staminaMax, float hungerMax, float thirstMax, float bodyTemp, int armorValue)
    {
        health = new Stat(healthMax, healthMax);
        stamina = new Stat(staminaMax, staminaMax);
        hunger = new Stat(hungerMax, hungerMax);
        thirst = new Stat(thirstMax, thirstMax);
        bodyTemperature = bodyTemp;
        armor = armorValue;
    }
}

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private float _regenStaminaRate = 2f;
    [SerializeField] private float _regenStaminaDelay = 1f;
    [SerializeField] private float _hungerDecayRate = 0.01f;
    [SerializeField] private float _thirstDecayRate = 0.01f;
    [SerializeField] private StatusPanel _statusPanel;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private ScreenTint _screenTint;

    public bool isDead = false;
    public bool isHungry = false;
    public bool isThirsty = false;
    public bool isExhausted = false;
    public bool isTemperatureCritical = false;

    private CheckPointInteract _currentCheckPoint;
    private Vector3 _checkPoint = Vector3.zero;
    private float _lastStaminaUseTime;
    private Animator _animator;

    public float GetCurrentHealth() => _playerStats.health.currentValue;
    public float GetCurrrentHunger() => _playerStats.hunger.currentValue;
    public float GetCurrentThirst() => _playerStats.thirst.currentValue;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        _playerStats = new PlayerStats(100f, 100f, 100f, 100f, 37f, 0);

        SetupStatusPanel(_statusPanel);
    }

    private void Update()
    {
        if (GameManager.instance.CurrentGameState != GameState.Playing) return;    

        if (Time.time - _lastStaminaUseTime >= _regenStaminaDelay)
        {
            _playerStats.stamina.Add(_regenStaminaRate * Time.deltaTime);
            if (_playerStats.stamina.currentValue >= _playerStats.stamina.maxValue)
            {
                isExhausted = false;
            }
        }

        Drain();

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(100f);
        }
    }

    private void Drain()
    {
        float hungerDrainRate = _hungerDecayRate;
        float thirstDrainRate = _thirstDecayRate;

        _playerStats.hunger.Subtract(hungerDrainRate * Time.deltaTime);
        _playerStats.thirst.Subtract(thirstDrainRate * Time.deltaTime);
        _statusPanel.UpdateHungerBar(_playerStats.hunger.currentValue, _playerStats.hunger.maxValue);        
        _statusPanel.UpdateThirstBar(_playerStats.thirst.currentValue, _playerStats.thirst.maxValue);
    }

    public void SetupStatusPanel(StatusPanel panel)
    {
        _statusPanel = panel;
        _statusPanel.SetUp(_playerStats);
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
        _statusPanel.UpdateHealthBar(_playerStats.health.currentValue, _playerStats.health.maxValue);
        
        if (_playerStats.health.currentValue <= 0 && !isDead)
        {
            isDead = true;
            _animator.SetTrigger("dead");
        }
    }

    public void Heal(float amount)
    {
        if (isDead) return;

        _playerStats.health.Add(amount);
        _statusPanel.UpdateHealthBar(_playerStats.health.currentValue, _playerStats.health.maxValue);
    }

    public void RestoreHunger(float amount)
    {
        _playerStats.hunger.Add(amount);
        _statusPanel.UpdateHungerBar(_playerStats.hunger.currentValue, _playerStats.hunger.maxValue);
        
        if (_playerStats.hunger.currentValue > 0f)
        {
            isHungry = false;
        }
    }

    public void RestoreThirst(float amount)
    {
        _playerStats.thirst.Add(amount);
        _statusPanel.UpdateThirstBar(_playerStats.thirst.currentValue, _playerStats.thirst.maxValue);
        
        if (_playerStats.thirst.currentValue > 0f)
        {
            isThirsty = false;
        }
    }

    public void SetRespawnPoint(Vector3 position, CheckPointInteract checkPointInteract)
    {
        _checkPoint = position + Vector3.down;
        if (_currentCheckPoint != null)
        {
            _currentCheckPoint.Deactivate();
        }
        _currentCheckPoint = checkPointInteract;
    }

    public void RespawnPlayer()
    {
        _screenTint.Tint();
        transform.position = _checkPoint;
        isDead = false;
        _playerStats.health.currentValue = _playerStats.health.maxValue / 2;
        _playerStats.hunger.currentValue = _playerStats.hunger.maxValue / 2;
        _playerStats.thirst.currentValue = _playerStats.thirst.maxValue / 2;
        SetupStatusPanel(_statusPanel);
        _screenTint.UnTint();
    }

    public void CalculateDamage(ref float damage)
    {

    }

    public void ApplyDamage(float damage)
    {
        //TakeDamage(damage);
    }

    public void CheckState()
    {

    }
}
