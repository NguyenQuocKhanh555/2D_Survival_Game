using UnityEngine;

public class PlayerUseItemController : MonoBehaviour
{
    [SerializeField] private PlayerToolbarController _toolbarController;
    [SerializeField] private string[] _directions;

    private Animator _animator;
    private AnimationClip _animationClip;
    private AnimatorOverrideController _animatorOverrideController;
    private AnimationClipOverrides _defaultAnimationClips;

    private SO_Tool _currentTool;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animatorOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);
        _animator.runtimeAnimatorController = _animatorOverrideController;

        _defaultAnimationClips = new AnimationClipOverrides(_animatorOverrideController.overridesCount);
        _animatorOverrideController.GetOverrides(_defaultAnimationClips);

        UpdateUseTool();
    }

    private void Update()
    {
        SO_Tool selectedTool = _toolbarController.GetToolbarSelectedItem.toolData;
        if (selectedTool != _currentTool)
        {
            _currentTool = selectedTool;
            UpdateUseTool();
        }
    }

    private void UpdateUseTool()
    {
        if (_currentTool == null) return;

        string toolName = _currentTool.toolName;
        string toolID = _currentTool.toolAnimationID.ToString();

        for (int directionIndex = 0; directionIndex < _directions.Length; directionIndex++)
        {
            string direction = _directions[directionIndex];
            _animationClip = Resources.Load<AnimationClip>(
                "PlayerAnimations/Tools/" + toolName + "s/" + toolName + "_" + toolID + "_" + direction);
            
            _defaultAnimationClips["Pickaxe_" + 0 + "_" + direction] = _animationClip;
        }

        _animatorOverrideController.ApplyOverrides(_defaultAnimationClips);
    }

    public void UseTool()
    {
        _animator.SetTrigger("action");
    }
}
