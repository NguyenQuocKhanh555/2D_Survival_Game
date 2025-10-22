using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private SO_ItemContainer _playerInventory;
    [SerializeField] private List<InventoryButton> _inventoryButtons;

    private void Start()
    {
        for (int i = 0; i < _inventoryButtons.Count; i++)
        {
            if (_playerInventory.itemSlots[i].item == null) { continue; }
            _inventoryButtons[i].SetIcon(_playerInventory.itemSlots[i].item.itemSprite);
            _inventoryButtons[i].SetQuantity(_playerInventory.itemSlots[i].quantity.ToString());
        }
    }
}
