using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Part", menuName = "Player Body Part")]
public class SO_PlayerBodyPart : ScriptableObject
{
    public string partName;
    public int partAnimationID;

    public List<AnimationClip> allPartAnimations = new List<AnimationClip>();
}
