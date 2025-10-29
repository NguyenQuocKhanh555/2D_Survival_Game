using System;
using UnityEngine;

public enum ItemEffectTypes
{
    Health,
    Food,
    Water
}

public enum ItemEffectMethods
{
    Add,
    Multiply
}

[Serializable]
public class ItemEffect
{
    public ItemEffectTypes effectType;
    public ItemEffectMethods effectMethod;
    public float effectValue;
    public float effectDuration;
}

[CreateAssetMenu(fileName = "New Item Effect", menuName = "Item Effect")]
public class SO_ItemEffect : ScriptableObject
{
    public ItemEffect[] itemEffects;
}
