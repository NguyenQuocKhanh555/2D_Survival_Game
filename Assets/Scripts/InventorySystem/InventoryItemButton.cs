using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItemButton : MonoBehaviour, IItemButton, IPointerClickHandler
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemQuantity;
    [SerializeField] private Sprite _lockIcon;

    private int _inventoryIndex;
    private bool _isLocked = false;

    private InventoryItemPanel _inventoryItemPanel;

    public void SetItemPanel(InventoryItemPanel source)
    {
        _inventoryItemPanel = source;
    }

    public void SetInventoryIndex(int index)
    {
        _inventoryIndex = index;
    }

    public void LockButton()
    {
        _isLocked = true;
    }

    public void UnlockButton()
    {
        _isLocked = false;
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

    public void SetLockIcon()
    {
        _itemIcon.gameObject.SetActive(true);
        _itemIcon.sprite = _lockIcon;
        _itemQuantity.gameObject.SetActive(false);
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
        if (_isLocked) return;
        _inventoryItemPanel.OnClick(_inventoryIndex);
    }
}
