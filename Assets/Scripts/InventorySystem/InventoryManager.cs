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

    public int CountSlotsNonEmpty()
    {
        int count = 0;
        foreach (var slot in _playerInventory.slots)
        {
            if (slot.item != null)
            {
                count++;
            }
        }
        return count;
    }

    public void ClearInventory()
    {
        _playerInventory.Clear();
        onInventoryChange?.Invoke();
    }

    public bool CheckFreeSpaceForStackableItem(SO_Item item, int checkAmount)
    {
        return _playerInventory.CheckFreeSpaceForStackableItem(item, checkAmount);
    }

    public bool CheckFreeSpaceForNonStackableItem(int checkQuantity)
    {
        return _playerInventory.CheckFreeSpaceForNonStackableItem(checkQuantity);
    }

    public int GetItemQuantity(SO_Item item)
    {
        return _playerInventory.GetItemQuantity(item);
    }
}
