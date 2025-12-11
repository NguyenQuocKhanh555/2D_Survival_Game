using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private SO_ItemContainer _playerInventory;

    public static InventoryManager instance;
    public Action onInventoryChange;
    public int inventorySize = 40;

    private void Awake()
    {
        instance = this;
    }

    public void AddItemToInventory(SO_Item item, int amount)
    {
        _playerInventory.AddItem(item, amount);
        onInventoryChange?.Invoke();
    }

    public void RemoveItemFromInventory(SO_Item item, int amount)
    {
        _playerInventory.RemoveItem(item, amount);
        onInventoryChange?.Invoke();
    }

    public ItemSlot GetItemSlotInInventory(int index)
    {
        return _playerInventory.slots[index];
    }

    public SO_Item GetItemInSlot(int index)
    {
        return _playerInventory.slots[index].item;
    }
    
    public int CountSlots()
    {
        return _playerInventory.slots.Count;
    }
}
