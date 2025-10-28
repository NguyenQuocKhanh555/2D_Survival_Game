using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class SO_Weapon : ScriptableObject
{
    public string weaponName;
    public int weaponAnimationID;
    public int weaponID;

    public List<AnimationClip> allWeaponAnimations = new List<AnimationClip>();
    
    public float weaponDamage;
}
