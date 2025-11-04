using UnityEngine;

public class EquipmentPreviewPanel : MonoBehaviour
{
    [SerializeField] private EquipmentItemSlotsPanel _equipmentItemSlotsPanel;
    [SerializeField] private string[] _bodyPartTypes;
    [SerializeField] private SO_PlayerBody _playerBody;

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

        UpdatePlayerPart();
    }

    private void OnEnable()
    {
        _equipmentItemSlotsPanel.onChangeEquipment += UpdatePlayerPart;
    }

    private void OnDisable()
    {
        _equipmentItemSlotsPanel.onChangeEquipment -= UpdatePlayerPart;
    }

    private void UpdatePlayerPart()
    {
        for (int i = 0; i < _playerBody.playerBodyParts.Length; i++)
        {
            if (_playerBody.playerBodyParts[i].playerPart == null)
            {
                _animationClip = null;
            }
            else
            {
                _animationClip = _playerBody.playerBodyParts[i].playerPart.previewPartIdleAnimation;
            }

            _defaultAnimationClips[_bodyPartTypes[i] + "_0_preview"] = _animationClip;
        }

        _animatorOverrideController.ApplyOverrides(_defaultAnimationClips);
    }
}
