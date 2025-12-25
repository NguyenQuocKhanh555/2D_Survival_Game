using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ItemSlot
{
    public SO_Item item;
    public int quantity;
    
    public ItemSlot() { }

    public ItemSlot(SO_Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public void Copy(ItemSlot other)
    {
        item = other.item;
        quantity = other.quantity;
    }

    public void TakeOne(ItemSlot other)
    {
        item = other.item;
        quantity = 1;
        other.quantity -= 1;
        if (other.quantity <= 0)
        {
            other.Clear();
        }
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

    public void AddItem(SO_Item itemToAdd, int addQuantity) 
    {
        isChange = true;

        if (itemToAdd.isStackable)
        {
            List<ItemSlot> itemSlots = slots.FindAll(x => x.item == itemToAdd);
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

                ItemSlot itemSlot = slots.Find(x => x.item == null);
                if (itemSlot != null)
                {
                    itemSlot.Set(itemToAdd, addQuantity);
                }
            }
            else
            {
                ItemSlot itemSlot = slots.Find(x => x.item == null);
                if (itemSlot != null)
                {
                    itemSlot.Set(itemToAdd, addQuantity);
                }
            }
        }
        else
        {
            List<ItemSlot> itemSlots = slots.FindAll(x => x.item == null);

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

    public void RemoveItem(SO_Item itemToRemove, int removeQuantity) 
    {
        List<ItemSlot> itemSlots = slots.FindAll(x => x.item == itemToRemove);

        if (itemSlots.Count <= 0) return;

        if (itemToRemove.isStackable)
        {      
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (removeQuantity <= 0) break;
                if (itemSlots[i].quantity <= removeQuantity)
                {
                    itemSlots[i].Clear();
                    removeQuantity -= itemSlots[i].quantity;
                }
                else
                {
                    itemSlots[i].quantity -= removeQuantity;
                    removeQuantity = 0;
                }
            }

            isChange = true;
        }
        else
        {
            if (slots.Count <= removeQuantity) return;
            
            for (int i = 0; i < removeQuantity; i++)
            {
                itemSlots[i].Clear();
            }

            isChange = true;
        }
    }

    public void AddItemSlot(ItemSlot itemSlotToAdd) 
    {
        slots.Add(new ItemSlot(itemSlotToAdd.item, itemSlotToAdd.quantity));
    }

    public bool CheckItem(ItemSlot itemToCheck) 
    {
        if (itemToCheck == null) return true;
        if (itemToCheck.item == null) return true;

        ItemSlot itemSlot = slots.Find(x => x.item == itemToCheck.item);

        if (itemSlot == null) return false;
        
        return true;
    }

    public int GetItemQuantity(SO_Item item)
    {
        List<ItemSlot> itemSlots = slots.FindAll(x => x.item == item);
        int count = 0;

        if (item.isStackable) 
        {
            if (itemSlots.Count <= 0) { return count; }
            
            for (int i = 0; i < itemSlots.Count; i++)
            {
                count += itemSlots[i].quantity;
            }
        }
        else
        {
            if (itemSlots.Count <= 0) { return count; }

            for (int i = 0; i < itemSlots.Count; i++)
            {
                count += 1;
            }
        }

        return count;
    }

    public bool CheckFreeSpaceForNonStackableItem(int checkQuantity)
    {
        List<ItemSlot> itemSlots = slots.FindAll(x => x.item == null);

        if (itemSlots.Count >= checkQuantity) return true;

        return false;
    }

    public bool CheckFreeSpaceForStackableItem(SO_Item item, int checkAmount)
    {
        if (checkAmount > item.maxStack) return true;

        List<ItemSlot> itemSlots = slots.FindAll(x => x.item == item);
        
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

            ItemSlot itemSlot = slots.Find(x => x.item == null);
            if (itemSlot != null) return true;
        }
        else
        {
            ItemSlot itemSlot = slots.Find(x => x.item == null);
            if (itemSlot != null) return true;
        }

        return false;
    }

    public void Init(int slotCount)
    {
        slots = new List<ItemSlot>();
        
        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(new ItemSlot());
        }
    }

    public void Clear()
    {
        foreach (ItemSlot slot in slots)
        {
            slot.Clear();
        }
    }
}
