using System;
using UnityEngine;

[Serializable]
public class Stat
{
    public int maxValue;
    public int currentValue;

    public Stat(int curr, int max)
    {
        currentValue = curr;
        maxValue = max;
    }

    public void Add(int amount)
    {
        currentValue += amount;
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }
    }

    public void Subtract(int amount)
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

public class Player : MonoBehaviour
{
    [SerializeField] private SO_PlayerStats playerStats;

    public bool isDead;
    public bool isHungry;
    public bool isThirsty;
    public bool isExhausted;
    public bool isTemperatureCritical;
}
