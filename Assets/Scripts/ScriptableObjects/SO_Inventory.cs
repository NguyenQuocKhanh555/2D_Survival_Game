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

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory")]
public class SO_Inventory : ScriptableObject
{
    public List<ItemSlot> items;

    public int GetTotalItemCount()
    {
        return items.Sum(slot => slot.quantity);
    }

    public List<ItemSlot> GetItemDetails()
    {
        return items;
    }
}
