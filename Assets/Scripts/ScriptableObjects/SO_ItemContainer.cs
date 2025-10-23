using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ItemSlot
{
    public SO_Item item;
    public int quantity;
    
    public void Copy(ItemSlot other)
    {
        item = other.item;
        quantity = other.quantity;
    }

    public void Set(SO_Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}

[CreateAssetMenu(fileName = "New Item Container", menuName = "Item Container")]
public class SO_ItemContainer : ScriptableObject
{
    public List<ItemSlot> slots;
    public bool isChange;

    public void AddItem(SO_Item itemToAdd, int quantity = 1) 
    {
        isChange = true;

        if (itemToAdd.isStackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == itemToAdd);
            if (itemSlot != null)
            {
                itemSlot.quantity += quantity;
            }
            else
            {
                itemSlot = slots.Find(x => x.item == null);
                if (itemSlot != null)
                {
                    itemSlot.Set(itemToAdd, quantity);
                }
            }
        }
        else
        {
            ItemSlot itemSlot = slots.Find(x => x.item == null);
            if (itemSlot != null)
            {
                itemSlot.Set(itemToAdd, 1);
            }
        }
    }

    public void RemoveItem(SO_Item itemToRemove, int quantity = 1) 
    {
        isChange = true;

        if (itemToRemove.isStackable)
        {
            ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
            if (itemSlot == null) return;

            itemSlot.quantity -= quantity;
            if (itemSlot.quantity <= 0)
            {
                itemSlot.Clear();
            }
        }
        else
        {
            while (quantity > 0)
            {
                quantity--;

                ItemSlot itemSlot = slots.Find(x => x.item == itemToRemove);
                if (itemSlot == null) return;
                itemSlot.Clear();
            }
        }
    }
}
