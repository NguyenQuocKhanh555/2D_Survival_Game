using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private SO_ItemContainer _playerInventory;
    [SerializeField] private Transform _player;

    public static InventoryManager instance;
    public Action onInventoryChange;
    public int inventorySize;

    public Action onInventorySizeChange;

    private void Awake()
    {
        instance = this;
    }

    public void PushItemOutOfLockedSlots(int newSize)
    {
        for (int i = inventorySize - 1; i >= newSize; i--)
        {
            ItemSlot itemSlot = _playerInventory.slots[i];
            
            if (itemSlot.item != null)
            {
                SpawnItemManager.instance.SpawnItem(_player.position + 
                    new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0), itemSlot.item, itemSlot.quantity);
                itemSlot.Clear();
            }
        }
    }

    public void AddItemToInventory(SO_Item itemToAdd, int addQuantity)
    {
        AddItem(itemToAdd, addQuantity);
        onInventoryChange?.Invoke();
    }

    private void AddItem(SO_Item itemToAdd, int addQuantity)
    {
        if (itemToAdd.isStackable)
        {
            if (CheckFreeSpaceForStackableItem(itemToAdd, addQuantity) == false)
            {
                SpawnItemManager.instance.SpawnItem(_player.position + 
                    new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0), itemToAdd, addQuantity);
                return;
            }

            List<ItemSlot> itemSlots = _playerInventory.slots.FindAll(x => x.item == itemToAdd);
            
            if (itemSlots.Count > 0)
            {
                for (int i = 0; i < itemSlots.Count; i++)
                {
                    if (itemSlots[i].quantity >= itemToAdd.maxStack) continue;

                    if (itemSlots[i].quantity + addQuantity > itemToAdd.maxStack)
                    {
                        addQuantity -= (itemToAdd.maxStack - itemSlots[i].quantity);
                        itemSlots[i].quantity = itemToAdd.maxStack;
                    }
                    else
                    {
                        itemSlots[i].quantity += addQuantity;
                        addQuantity = 0;
                        break;
                    }
                }

                if (addQuantity <= 0) return;

                ItemSlot itemSlot = FindEmptySlot();
                if (itemSlot != null)
                {
                    itemSlot.Set(itemToAdd, addQuantity);
                }
            }
            else
            {
                ItemSlot itemSlot = FindEmptySlot();
                if (itemSlot != null)
                {
                    itemSlot.Set(itemToAdd, addQuantity);
                }
            }
        }
        else
        {
            List<ItemSlot> itemSlots = FindListEmptySlot();

            if (addQuantity <= 0)
            {
                addQuantity = 1;
            }

            for (int i = 0; i < addQuantity; i++)
            {
                itemSlots[i].Set(itemToAdd, 0);
            }
        } 
    }

    private ItemSlot FindEmptySlot()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (_playerInventory.slots[i].item == null)
            {
                return _playerInventory.slots[i];
            }
        }

        return null;
    }

    private List<ItemSlot> FindListEmptySlot()
    {
        List<ItemSlot> emptySlots = new List<ItemSlot>();
        
        for (int i = 0; i < inventorySize; i++)
        {
            if (_playerInventory.slots[i].item == null)
            {
                emptySlots.Add(_playerInventory.slots[i]);
            }
        }
        
        return emptySlots;
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
        //if (checkAmount > item.maxStack) return true;

        List<ItemSlot> itemSlots = _playerInventory.slots.FindAll(x => x.item == item);

        if (itemSlots.Count > 0)
        {
            int itemQuantity = 0;

            for (int i = 0; i < itemSlots.Count; i++)
            {
                itemQuantity += itemSlots[i].quantity;
            }

            if ((item.maxStack * itemSlots.Count) - itemQuantity >= checkAmount)
            {
                return true;
            }

            ItemSlot itemSlot = FindEmptySlot();
            if (itemSlot != null) return true;
        }
        else
        {
            ItemSlot itemSlot = FindEmptySlot();
            if (itemSlot != null) return true;
        }

        return false;
    }

    public bool CheckFreeSpaceForNonStackableItem(int checkQuantity)
    {
        List<ItemSlot> itemSlots = FindListEmptySlot();

        if (itemSlots.Count >= checkQuantity) return true;

        return false;
    }

    public int GetItemQuantity(SO_Item item)
    {
        return _playerInventory.GetItemQuantity(item);
    }
}
