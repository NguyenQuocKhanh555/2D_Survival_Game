using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootContainerUIButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TMP_Text _itemQuantity;

    private int _lootContainerIndex;

    private LootContainerUI _lootContainerUI;

    public void SetItemPanel(LootContainerUI source)
    {
        _lootContainerUI = source;
    }

    public void SetInventoryIndex(int index)
    {
        _lootContainerIndex = index;
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
        _lootContainerUI.OnClick(_lootContainerIndex);
    }
}
