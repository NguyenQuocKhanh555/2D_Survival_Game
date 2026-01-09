using System.Collections.Generic;
using UnityEngine;

public class PlayerPassiveController : MonoBehaviour
{
    [SerializeField] private Player _player;

    private HashSet<PassiveEffectTypes> _activePassives = new HashSet<PassiveEffectTypes>();

    public bool HasPassiveEffect(PassiveEffectTypes effectType)
    {
        return _activePassives.Contains(effectType);
    }

    public void AddPassiveEffect(PassiveEffectTypes effectType)
    {
        _activePassives.Add(effectType);

        switch (effectType)
        {
            case PassiveEffectTypes.HeatResistance:
                _player.ResetTemperature();
                break;
            case PassiveEffectTypes.ColdResistance:
                _player.ResetTemperature();
                break;
        }
    }

    public void RemovePassiveEffect(PassiveEffectTypes effectType)
    {
        _activePassives.Remove(effectType);
    }
}
