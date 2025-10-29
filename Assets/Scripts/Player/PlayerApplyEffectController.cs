using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerApplyEffectController : MonoBehaviour
{
    [SerializeField] private SO_PlayerStats _playerStats;

    private readonly Dictionary<ItemEffectTypes, Action<ItemEffect>> _instantHandlers;
    private readonly Dictionary<ItemEffectTypes, Action<ItemEffect>> _overTimeHandlers;

    public PlayerApplyEffectController()
    {
        _instantHandlers = new Dictionary<ItemEffectTypes, Action<ItemEffect>>
        {
            { ItemEffectTypes.Health, ApplyHealthInstant },
            { ItemEffectTypes.Food, ApplyFoodInstant },
            { ItemEffectTypes.Water, ApplyWaterInstant }
        };

        _overTimeHandlers = new Dictionary<ItemEffectTypes, Action<ItemEffect>>
        {
            { ItemEffectTypes.Health, ApplyHealthOverTime },
            { ItemEffectTypes.Food, ApplyFoodOverTime },
            { ItemEffectTypes.Water, ApplyWaterOverTime }
        };
    }

    public void ApplyEffect(SO_ItemEffect itemEffect)
    {
        if (itemEffect == null)
        {
            return;
        }

        foreach (ItemEffect effect in itemEffect.itemEffects)
        {
            var handlers = effect.effectDuration > 0 ? _overTimeHandlers : _instantHandlers;

            if (handlers.TryGetValue(effect.effectType, out var handler))
            {
                handler(effect);
            }
        }
    }

    // Instant
    private void ApplyHealthInstant(ItemEffect effect)
    {
        _playerStats.health.Add(ModifyStat(_playerStats.health.currentValue, effect));
    }

    private void ApplyFoodInstant(ItemEffect effect)
    {
        _playerStats.hunger.Add(ModifyStat(_playerStats.hunger.currentValue, effect));
    }

    private void ApplyWaterInstant(ItemEffect effect)
    {
        _playerStats.thirst.Add(ModifyStat(_playerStats.thirst.currentValue, effect));
    }

    // Over Time

    private void ApplyHealthOverTime(ItemEffect effect)
    {
        StartCoroutine(ApplyOverTime(effect,
            () => _playerStats.health.Add(ModifyStat(_playerStats.health.currentValue, effect))));
    }

    private void ApplyFoodOverTime(ItemEffect effect)
    {
        StartCoroutine(ApplyOverTime(effect,
            () => _playerStats.hunger.Add(ModifyStat(_playerStats.hunger.currentValue, effect))));
    }

    private void ApplyWaterOverTime(ItemEffect effect)
    {
        StartCoroutine(ApplyOverTime(effect,
            () => _playerStats.thirst.Add(ModifyStat(_playerStats.thirst.currentValue, effect))));
    }

    private IEnumerator ApplyOverTime(ItemEffect effect, Action tickAction)
    {
        float elapsed = 0f;

        while (elapsed < effect.effectDuration)
        {
            tickAction.Invoke();
            yield return new WaitForSeconds(1);
            elapsed++;
        }
    }

    private float ModifyStat(float currentValue, ItemEffect effect)
    {
        return effect.effectMethod switch
        {
            ItemEffectMethods.Add => effect.effectValue,
            ItemEffectMethods.Multiply => currentValue * effect.effectValue,
            _ => 0f
        };
    }

}
