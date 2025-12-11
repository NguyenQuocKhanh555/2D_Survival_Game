using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerToolbarController : MonoBehaviour
{
    [SerializeField] private int _toolbarSize = 12;

    private int _selectedToolbarIndex;

    public Action<int> onToolbarIndexChanged;
    public Action onToolbarSelectedChanged;

    public ItemSlot GetToolbarSelectedItemSlot
    {
        get { return InventoryManager.instance.GetItemSlotInInventory(_selectedToolbarIndex); }
    }

    public SO_Item GetToolbarSelectedItem
    {
        get { return InventoryManager.instance.GetItemInSlot(_selectedToolbarIndex); }
    }

    public void RemoveItem(SO_Item itemToRemove)
    {
        InventoryManager.instance.RemoveItemFromInventory(itemToRemove, 1);
    }

    public void Set(int id)
    {
        _selectedToolbarIndex = id;
        onToolbarIndexChanged?.Invoke(_selectedToolbarIndex);
        onToolbarSelectedChanged?.Invoke();
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
