using UnityEngine;

public class GraveInteract : Interactable
{
    [SerializeField] private SO_ItemContainer _graveContainer;

    public void StoreInventory()
    {
        int size = InventoryManager.instance.CountSlotsNonEmpty();
        _graveContainer = (SO_ItemContainer)ScriptableObject.CreateInstance(typeof(SO_ItemContainer));
        _graveContainer.Init(size);

        for (int i = 0; i < 40; i++)
        {
            if (InventoryManager.instance.GetItemInSlot(i) != null)
            {
                ItemSlot slot = InventoryManager.instance.GetItemSlotInInventory(i);
                _graveContainer.AddItem(slot.item, slot.quantity);
            }
        }

        InventoryManager.instance.ClearInventory();
    }

    public override void Interact(Player player)
    {
        for (int i = 0; i < _graveContainer.slots.Count; i++)
        {
            ItemSlot slot = _graveContainer.slots[i];
            
            if (slot.item != null)
            {
                InventoryManager.instance.AddItemToInventory(slot.item, slot.quantity);
            }
        }

        Destroy(gameObject);
    }
}
