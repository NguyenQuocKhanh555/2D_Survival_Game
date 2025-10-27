using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New tool", menuName = "Tool")]
public class SO_Tool : ScriptableObject
{
    public string toolName;
    public int toolAnimationID;

    public List<AnimationClip> allToolAnimations = new List<AnimationClip>();
}
