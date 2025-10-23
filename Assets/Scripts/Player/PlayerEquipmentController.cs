using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentController : MonoBehaviour
{
    [SerializeField] private SO_PlayerBody _playerBody;

    [SerializeField] private string[] _bodyPartTypes;
    [SerializeField] private string[] _playerStates;
    [SerializeField] private string[] _playerDirections;

    private Animator _animator;
    private AnimationClip _animationClip;
    private AnimatorOverrideController _animatorOverrideController;
    private AnimationClipOverrides _defaultAnimationClips;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;

        _defaultAnimationClips = new AnimationClipOverrides(_animatorOverrideController.overridesCount);
        _animatorOverrideController.GetOverrides(_defaultAnimationClips);

        UpdatePlayerParts();
    }

    private void UpdatePlayerParts()
    {
        for (int partIndex = 0; partIndex < _bodyPartTypes.Length; partIndex++)
        {
            string partType = _bodyPartTypes[partIndex];
            string partID = _playerBody.playerBodyParts[partIndex].playerPart.partAnimationID.ToString();

            for (int directionIndex = 0; directionIndex < _playerDirections.Length; directionIndex++)
            {
                string direction = _playerDirections[directionIndex];

                for (int stateIndex = 0; stateIndex < _playerStates.Length; stateIndex++)
                {
                    string state = _playerStates[stateIndex];

                    _animationClip = Resources.Load<AnimationClip>(
                        "PlayerAnimations/" + state + "/" + partType + "/" + partType + "_" + partID + "_" + state + "_" + direction);

                    _defaultAnimationClips[partType + "_" + 0 + "_" + state + "_" + direction] = _animationClip;
                }
            }
        }

        _animatorOverrideController.ApplyOverrides(_defaultAnimationClips);
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
