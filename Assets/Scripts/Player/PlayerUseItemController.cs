using UnityEngine;

public class PlayerUseItemController : MonoBehaviour
{
    [SerializeField] private TilemapReadController _tilemapReadController;
    [SerializeField] private PlayerEquipmentController _equipmentController;
    [SerializeField] private PlayerAttackController _attackController;
    [SerializeField] private PlayerApplyEffectController _applyEffectController;
    [SerializeField] private float _offSetDistance = 1f;
    [SerializeField] private float _selectableDistance = 1f;

    private Vector3Int _selectedTilePosition;
    private bool _isSelectable;

    public void UseTool(Animator animator, Vector2 lastMotionVector)
    {
        if (_equipmentController.currentToolData == null) return;
        Vector2 position = (Vector2)transform.position + lastMotionVector * _offSetDistance;

        animator.SetTrigger("action");

        if (_equipmentController.currentToolData.toolWorldAction != null)
        {
            _equipmentController.currentToolData.toolWorldAction.OnApply(
                position, _equipmentController.currentToolData.toolPower);
        }
        else
        {
            if (!_isSelectable) return;
            _equipmentController.currentToolData.toolTileMapAction.OnApplyToTileMap(
                _selectedTilePosition, _tilemapReadController);
        }
    }

    public void SelectTile()
    {
        _selectedTilePosition = _tilemapReadController.GetGridPosition(Input.mousePosition, true);
    }

    public void CanSelectCheck()
    {
        Vector2 playerPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isSelectable = Vector2.Distance(playerPosition, cameraPosition) < _selectableDistance;
    }

    public void UseWeapon(Animator animator, Vector2 lastMotionVector)
    {
        if (_equipmentController.currentWeaponData == null) return;
        animator.SetTrigger("attack");
        animator.SetInteger("weaponId", _equipmentController.currentWeaponData.weaponID);
        _attackController.MeleeAttack(
            _equipmentController.currentWeaponData.weaponDamage, lastMotionVector);
    }

    public void UseFishingRod(Animator animator)
    {
        if (_equipmentController.currentToolData == null) return;
        animator.SetTrigger("fish");
    }

    public void UseConsumableItem(Animator animator, Vector2 lastMotionVector, PlayerToolbarController toolbarController)
    {
        SO_Item consumeItem = toolbarController.GetToolbarSelectedItem;

        if (consumeItem == null || consumeItem.itemType != ItemTypes.Consumable) return;

        animator.SetTrigger("consume");
        _applyEffectController.ApplyEffect(consumeItem.itemEffect);
        toolbarController.RemoveItem(consumeItem);
    }
}
