using System.Collections.Generic;
using UnityEngine;

public enum PlayerBodyPartType
{
    Head,
    Torso,
    Legs,
    Backpack
}

[CreateAssetMenu(fileName = "New Player Part", menuName = "Player Body Part")]
public class SO_PlayerBodyPart : ScriptableObject
{
    //public string partName;
    public PlayerBodyPartType partName;
    public int partAnimationID;

    public List<AnimationClip> allPartAnimations = new List<AnimationClip>();

    public AnimationClip previewPartIdleAnimation;
}
