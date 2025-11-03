using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentItemButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _itemIcon;

    private int _equipmentSlotIndex;

    private EquipmentItemSlotsPanel _equipmentItemSlotsPanel;

    public void SetItemPanel(EquipmentItemSlotsPanel source)
    {
        _equipmentItemSlotsPanel = source;
    }

    public void SetEquipmentSlotIndex(int index)
    {
        _equipmentSlotIndex = index;
    }

    public void Set(ItemSlot itemSlot)
    {
        _itemIcon.gameObject.SetActive(true);
        _itemIcon.sprite = itemSlot.item.itemIcon;
    }

    public void Clear()
    {
        _itemIcon.sprite = null;
        _itemIcon.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _equipmentItemSlotsPanel.OnClick(_equipmentSlotIndex);
    }
}
