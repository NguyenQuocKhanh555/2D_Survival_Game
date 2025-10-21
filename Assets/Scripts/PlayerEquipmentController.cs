using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentController : MonoBehaviour
{
    [SerializeField] private SO_PlayerBody playerBody;

    [SerializeField] private string[] _bodyPartTypes;
    [SerializeField] private string[] _playerStates;
    [SerializeField] private string[] _playerDirections;

    private Animator animator;
    private AnimationClip animationClip;
    private AnimatorOverrideController animatorOverrideController;
    private AnimationClipOverrides defaultAnimationClips;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = animatorOverrideController;

        defaultAnimationClips = new AnimationClipOverrides(animatorOverrideController.overridesCount);
        animatorOverrideController.GetOverrides(defaultAnimationClips);

        UpdatePlayerParts();
    }

    private void UpdatePlayerParts()
    {
        for (int partIndex = 0; partIndex < _bodyPartTypes.Length; partIndex++)
        {
            string partType = _bodyPartTypes[partIndex];
            string partID = playerBody.playerBodyParts[partIndex].playerPart.partAnimationID.ToString();

            for (int stateIndex = 0; stateIndex < _playerStates.Length; stateIndex++)
            {
                string state = _playerStates[stateIndex];

                for (int directionIndex = 0; directionIndex < _playerDirections.Length; directionIndex++)
                {
                    string direction = _playerDirections[directionIndex];

                    animationClip = Resources.Load<AnimationClip>(
                        "PlayerAnimations/" + partType + "/" + state + "/" + partType + "_" + partID + "_" + state + "_" + direction);

                    defaultAnimationClips[partType + "_" + 0 + "_" + state + "_" + direction] = animationClip;
                }
            }
        }

        animatorOverrideController.ApplyOverrides(defaultAnimationClips);
    }
}

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
{
    public AnimationClipOverrides(int capacity) : base(capacity) { }
    
    public AnimationClip this[string name]
    {
        get
        {
            return this.Find(x => x.Key.name.Equals(name)).Value;
        }
        set
        {
            int index = this.FindIndex(x => x.Key.name.Equals(name));
            if (index != -1)
            {
                this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }
}
