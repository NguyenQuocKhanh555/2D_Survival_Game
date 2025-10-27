using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerToolbarController : MonoBehaviour
{
    [SerializeField] private int _toolbarSize = 12;
    [SerializeField] private SO_ItemContainer _inventoryContainer;

    private int _selectedToolbarIndex;

    public Action<int> onToolbarIndexChanged;
    public Action onToolbarSelectedChanged;

    public ItemSlot GetToolbarSelectedItemSlot
    {
        get { return _inventoryContainer.slots[_selectedToolbarIndex]; }
    }

    public SO_Item GetToolbarSelectedItem
    {
        get { return _inventoryContainer.slots[_selectedToolbarIndex].item; }
    }

    public void SelectToolbarIndex(float delta)
    {
        if (delta < 0)
        {
            _selectedToolbarIndex++;
            _selectedToolbarIndex = _selectedToolbarIndex >= _toolbarSize ? 0 : _selectedToolbarIndex;
        }
        else
        {
            _selectedToolbarIndex--;
            _selectedToolbarIndex = _selectedToolbarIndex < 0 ? _toolbarSize - 1 : _selectedToolbarIndex;
        }
        onToolbarIndexChanged?.Invoke(_selectedToolbarIndex);
        onToolbarSelectedChanged?.Invoke();
    }
}
