using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolbarItemButton : MonoBehaviour, IItemButton, IPointerClickHandler
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemQuantity;
    [SerializeField] private Image _selectButtonHighlight;
    [SerializeField] private Image _hoverButtonHighlight;

    private int _toolbarIndex;

    private ToolbarItemPanel _toolbarItemPanel;

    public void SetItemPanel(ToolbarItemPanel source)
    {
        _toolbarItemPanel = source;
    }

    public void SetToolbarIndex(int index)
    {
        _toolbarIndex = index;
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
        _toolbarItemPanel.OnClick(_toolbarIndex);
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

    public void SelectedHighlight(bool isSelected)
    {
        _selectButtonHighlight.gameObject.SetActive(isSelected);
    }
}
