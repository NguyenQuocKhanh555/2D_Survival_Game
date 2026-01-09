using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerApplyEffectController : MonoBehaviour
{
    [SerializeField] private Player _player;

    private Dictionary<ItemEffectTypes, Action<ItemEffect>> _instantHandlers;
    private Dictionary<ItemEffectTypes, Action<ItemEffect>> _overTimeHandlers;

    private void Start()
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
        _player.Heal(ModifyStat(_player.GetCurrentHealth(), effect));
    }

    private void ApplyFoodInstant(ItemEffect effect)
    {
        _player.RestoreHunger(ModifyStat(_player.GetCurrrentHunger(), effect));
    }

    private void ApplyWaterInstant(ItemEffect effect)
    {
        _player.RestoreThirst(ModifyStat(_player.GetCurrentThirst(), effect));
    }

    // Over Time
    private void ApplyHealthOverTime(ItemEffect effect)
    {
        StartCoroutine(ApplyOverTime(effect,
            () => _player.Heal(ModifyStat(_player.GetCurrentHealth(), effect))));
    }

    private void ApplyFoodOverTime(ItemEffect effect)
    {
        StartCoroutine(ApplyOverTime(effect,
            () => _player.RestoreHunger(ModifyStat(_player.GetCurrrentHunger(), effect))));
    }

    private void ApplyWaterOverTime(ItemEffect effect)
    {
        StartCoroutine(ApplyOverTime(effect,
            () => _player.RestoreThirst(ModifyStat(_player.GetCurrentThirst(), effect))));
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
