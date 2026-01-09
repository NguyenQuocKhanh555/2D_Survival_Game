using System;
using UnityEngine;

public enum PassiveEffectTypes
{
    HeatResistance,
    ColdResistance
}

[Serializable]
public class PassiveEffect
{
    public PassiveEffectTypes effectType;
}

[CreateAssetMenu(fileName = "New Item Set", menuName = "Item Set")]
public class SO_ItemSet : ScriptableObject
{
    public int requiredPieces;
    public PassiveEffect passiveEffects;
}
