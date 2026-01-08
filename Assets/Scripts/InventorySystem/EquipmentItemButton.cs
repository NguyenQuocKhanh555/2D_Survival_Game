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

    public void Set(Sprite sprite)
    {
        _itemIcon.sprite = sprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _equipmentItemSlotsPanel.OnClick(_equipmentSlotIndex);
    }
}
