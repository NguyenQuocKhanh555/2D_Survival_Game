using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentController : MonoBehaviour
{
    [SerializeField] private SO_PlayerBody _playerBody;
    [SerializeField] private PlayerToolbarController _toolbarController;
    [SerializeField] private EquipmentItemSlotsPanel _equipmentItemSlotsPanel;

    [SerializeField] private string[] _bodyPartTypes;
    [SerializeField] private string[] _playerStates;
    [SerializeField] private string[] _playerDirections;
    [SerializeField] private string[] _fishingAction;

    private Animator _animator;
    private AnimationClip _animationClip;
    private AnimatorOverrideController _animatorOverrideController;
    private AnimationClipOverrides _defaultAnimationClips;

    public SO_Tool currentToolData;
    public SO_Weapon currentWeaponData;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;

        _defaultAnimationClips = new AnimationClipOverrides(_animatorOverrideController.overridesCount);
        _animatorOverrideController.GetOverrides(_defaultAnimationClips);

        UpdatePlayerParts();
        UpdateUseTool();
        UpdateUseWeapon();

        _equipmentItemSlotsPanel.onChangeEquipment += UpdatePlayerParts;
        _toolbarController.onToolbarSelectedChanged += UseToolChanged;
        _toolbarController.onToolbarSelectedChanged += UseWeaponChanged;
    }

    private void UpdatePlayerParts()
    {
        for (int partIndex = 0; partIndex < _bodyPartTypes.Length; partIndex++)
        {
            string partType = _bodyPartTypes[partIndex];
            string partID = "";

            if (_playerBody.playerBodyParts[partIndex].playerPart == null) 
            {
                partID = "0";
            }
            else
            {
                partID = _playerBody.playerBodyParts[partIndex].playerPart.partAnimationID.ToString();
            }

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

    private void UpdateUseTool()
    {
        if (currentToolData == null) return;

        string toolName = currentToolData.toolName;
        string toolAnimationID = currentToolData.toolAnimationID.ToString();

        if (currentToolData.toolID == 2)
        {
            UpdateUseFishingRod(toolAnimationID);
            return;
        }

        for (int directionIndex = 0; directionIndex < _playerDirections.Length; directionIndex++)
        {
            string direction = _playerDirections[directionIndex];
            _animationClip = Resources.Load<AnimationClip>(
                "PlayerAnimations/Tools/" + toolName + "s/" + toolName + "_" + toolAnimationID + "_" + direction);

            _defaultAnimationClips["Pickaxe_" + 0 + "_" + direction] = _animationClip;
        }

        _animatorOverrideController.ApplyOverrides(_defaultAnimationClips);
    }

    private void UpdateUseFishingRod(string toolAnimationID)
    {
        for (int directionIndex = 0; directionIndex < _playerDirections.Length; directionIndex++)
        {
            string direction = _playerDirections[directionIndex];

            for (int actionIndex = 0; actionIndex < _fishingAction.Length; actionIndex++)
            {
                string action = _fishingAction[actionIndex];

                _animationClip = Resources.Load<AnimationClip>(
                "PlayerAnimations/FishingRod/" + action + "/" + action + "_" + toolAnimationID + "_" + direction);

                _defaultAnimationClips[action + "_" + 0 + "_" + direction] = _animationClip;
            }        
        }

        _animatorOverrideController.ApplyOverrides(_defaultAnimationClips);
    }

    private void UpdateUseWeapon()
    {
        if (currentWeaponData == null) return;

        string weaponName = currentWeaponData.weaponName;
        string weaponAnimationID = currentWeaponData.weaponAnimationID.ToString();

        for (int directionIndex = 0; directionIndex < _playerDirections.Length; directionIndex++)
        {
            string direction = _playerDirections[directionIndex];
            _animationClip = Resources.Load<AnimationClip>(
                "PlayerAnimations/Weapons/" + weaponName + "s/" + weaponName + "_" + weaponAnimationID + "_" + direction);

            _defaultAnimationClips["Sword_" + 0 + "_" + direction] = _animationClip;
        }

        _animatorOverrideController.ApplyOverrides(_defaultAnimationClips);
    }

    private void UseToolChanged()
    {
        if (_toolbarController.GetToolbarSelectedItem == null) return;
        SO_Tool selectedTool = _toolbarController.GetToolbarSelectedItem.toolData;
        if (selectedTool != currentToolData && selectedTool != null)
        {
            currentToolData = selectedTool;
            UpdateUseTool();
        }
    }

    private void UseWeaponChanged()
    {
        if (_toolbarController.GetToolbarSelectedItem == null) return;
        SO_Weapon selectedWeapon = _toolbarController.GetToolbarSelectedItem.weaponData;
        if (selectedWeapon != currentToolData && selectedWeapon != null)
        {
            currentWeaponData = selectedWeapon;
            UpdateUseWeapon();
        }
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
