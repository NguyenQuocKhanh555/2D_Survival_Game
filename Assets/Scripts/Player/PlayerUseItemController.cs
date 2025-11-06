using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItemController : MonoBehaviour
{
    [SerializeField] private TilemapReadController _tilemapReadController;
    [SerializeField] private PlaceableObjectsManager _placeableObjectsManager;
    [SerializeField] private PlayerEquipmentController _equipmentController;
    [SerializeField] private PlayerAttackController _attackController;
    [SerializeField] private PlayerApplyEffectController _applyEffectController;
    [SerializeField] private MarkerManager _markerManager;
    [SerializeField] private PlacementPreview _placementPreview;
    [SerializeField] private float _offSetDistance = 1f;
    [SerializeField] private float _selectableDistance = 1f;

    private Vector3Int _selectedTilePosition;
    private List<Vector3Int> _selectedTileArea;
    private bool _isOutRangeSelectable;
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
            if (!_isOutRangeSelectable) return;
            _equipmentController.currentToolData.toolTileMapAction.OnApplyToTileMap(
                _selectedTilePosition, _tilemapReadController);
        }
    }

    public void SelectTile(PlayerToolbarController playerToolbarController)
    {
        SO_Item selectedItem = playerToolbarController.GetToolbarSelectedItem;

        if (selectedItem != null && selectedItem.itemType == ItemTypes.Placeable)
        {
            Vector2Int itemSize = selectedItem.placeableData.sizeOnGrid;
            _selectedTileArea = _tilemapReadController.GetGridArea(Input.mousePosition, true, itemSize);
        }
        else
        {
            _selectedTilePosition = _tilemapReadController.GetGridPosition(Input.mousePosition, true);
        }
    }

    public void CanSelectCheck()
    {
        Vector2 playerPosition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _isOutRangeSelectable = Vector2.Distance(playerPosition, cameraPosition) < _selectableDistance;
        _markerManager.ShowMarker(_isOutRangeSelectable);
        _placementPreview.CanSelect = _isOutRangeSelectable;
    }

    public void Marker(PlayerToolbarController playerToolbarController)
    {
        SO_Item selectedItem = playerToolbarController.GetToolbarSelectedItem;

        if (selectedItem != null && selectedItem.itemType == ItemTypes.Placeable)
        {
            _markerManager.markedCellArea = _selectedTileArea;
            _markerManager.markedCellPosition = _selectedTileArea != null && _selectedTileArea.Count > 0
                ? _selectedTileArea[0]
                : Vector3Int.zero;

            _placementPreview.cellPosition = _selectedTileArea[0];
        }
        else
        {
            _markerManager.markedCellPosition = _selectedTilePosition;
            _markerManager.markedCellArea = new List<Vector3Int> { _selectedTilePosition };
        }
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
        if (!_isOutRangeSelectable) return;
        if (_placeableObjectsManager.Check(_selectedTileArea)) return;

        CalculatePlayerFaceDirection(animator);
        animator.SetTrigger("place");
        _placeableObjectsManager.Place(placeableItem, _selectedTileArea);
        toolbarController.RemoveItem(placeableItem);
        toolbarController.onToolbarSelectedChanged?.Invoke();
    }

    private void CalculatePlayerFaceDirection(Animator animator)
    {
        Vector2 direction = (Vector2)(_selectedTilePosition - transform.position).normalized;

        Vector2 finalDirection;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            finalDirection = direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            finalDirection = direction.y > 0 ? Vector2.up : Vector2.down;
        }

        animator.SetFloat("lastHorizontal", finalDirection.x);
        animator.SetFloat("lastVertical", finalDirection.y);
    }
}
