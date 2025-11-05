using UnityEngine;

public class PlayerUseItemController : MonoBehaviour
{
    [SerializeField] private TilemapReadController _tilemapReadController;
    [SerializeField] private PlaceableObjectsManager _placeableObjectsManager;
    [SerializeField] private PlayerEquipmentController _equipmentController;
    [SerializeField] private PlayerAttackController _attackController;
    [SerializeField] private PlayerApplyEffectController _applyEffectController;
    [SerializeField] private MarkerManager _markerManager;
    [SerializeField] private float _offSetDistance = 1f;
    [SerializeField] private float _selectableDistance = 1f;

    private Vector3Int _selectedTilePosition;
    private bool _isSelectable;

    public void UseTool(Animator animator, Vector2 lastMotionVector)
    {
        if (_equipmentController.currentToolData == null) return;
        
        animator.SetTrigger("action");

        if (_equipmentController.currentToolData.toolWorldAction != null)
        {
            Vector2 position = (Vector2)transform.position + lastMotionVector * _offSetDistance;
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
        _markerManager.ShowMarker(_isSelectable);
    }

    public void Marker()
    {
        _markerManager.markedCellPosition = _selectedTilePosition;
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

    public void UsePlaceableItem(Animator animator,PlayerToolbarController toolbarController)
    {
        SO_Item placeableItem = toolbarController.GetToolbarSelectedItem;
        
        if (placeableItem == null || placeableItem.itemType != ItemTypes.Placeable) return;
        if (!_isSelectable) return;
        if (_placeableObjectsManager.Check(_selectedTilePosition)) return;

        CalculatePlayerFaceDirection(animator);
        animator.SetTrigger("action");
        _placeableObjectsManager.Place(placeableItem, _selectedTilePosition);
        toolbarController.RemoveItem(placeableItem);
    }

    private void CalculatePlayerFaceDirection(Animator animator)
    {
        Vector2 direction = (Vector2)(_selectedTilePosition - transform.position).normalized;
        animator.SetFloat("lastHorizontal", direction.x);
        animator.SetFloat("lastVertical", direction.y);
    }
}
