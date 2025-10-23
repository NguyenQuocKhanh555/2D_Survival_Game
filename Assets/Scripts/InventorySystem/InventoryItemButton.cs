using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemButton : MonoBehaviour, IItemButton, IPointerClickHandler
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemQuantity;

    private int _inventoryIndex;

    private InventoryItemPanel _itemPanel;

    public void SetItemPanel(InventoryItemPanel source)
    {
        _itemPanel = source;
    }

    public void SetInventoryIndex(int index)
    {
        _inventoryIndex = index;
    }

    public void Set(ItemSlot itemSlot)
    {
        _itemIcon.gameObject.SetActive(true);
        _itemIcon.sprite = itemSlot.item.itemIcon;

        if (itemSlot.item.isStackable)
        {
            _itemQuantity.gameObject.SetActive(true);
            _itemQuantity.text = itemSlot.quantity.ToString();
        }
        else
        {
            _itemQuantity.gameObject.SetActive(false);
        }
    }

    public void Clear()
    {
        _itemIcon.sprite = null;
        _itemIcon.gameObject.SetActive(false);
        _itemQuantity.text = string.Empty;
        _itemQuantity.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _itemPanel.OnClick(_inventoryIndex);
    }
}
