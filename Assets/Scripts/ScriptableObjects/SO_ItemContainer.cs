using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ItemSlot
{
    public SO_Item item;
    public int quantity;
    public ItemSlot(SO_Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

}

[CreateAssetMenu(fileName = "New Item Container", menuName = "Item Container")]
public class SO_ItemContainer : ScriptableObject
{
    public List<ItemSlot> itemSlots;

    public int GetTotalItemCount()
    {
        return itemSlots.Sum(slot => slot.quantity);
    }

    public List<ItemSlot> GetItemDetails()
    {
        return itemSlots;
    }
}
