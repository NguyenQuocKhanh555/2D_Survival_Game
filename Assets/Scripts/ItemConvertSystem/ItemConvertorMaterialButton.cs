using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemConvertorMaterialButton : MonoBehaviour
{
    [SerializeField] private ItemConvertorUI _itemConvertorUI;
    [SerializeField] private Image _materialIcon;
    [SerializeField] private TMP_Text _quantity;
    [SerializeField] private ItemDragAndDrop _dragAndDrop;
    [SerializeField] private Button _materialButton;

    [SerializeField] private ItemSlot _materialSlot;
    [SerializeField] private int _materialSlotIndex; 

    private void Start()
    {
        _materialButton.onClick.AddListener(OnClick);
        _materialSlot = new ItemSlot();
    }

    public void SetIndex(int index)
    {
        _materialSlotIndex = index;
    }

    public void Show(ItemSlot slot)
    {
        _materialSlot.Copy(slot);
        _materialIcon.gameObject.SetActive(true);
        _materialIcon.sprite = _materialSlot.item.itemIcon;
        _quantity.gameObject.SetActive(true);
        _quantity.text = _materialSlot.quantity.ToString();
    }

    public void Clear()
    {
        _materialIcon.sprite = null;
        _materialIcon.gameObject.SetActive(false);
        _quantity.text = "";
        _quantity.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        _dragAndDrop.OnClick(_materialSlot);
        _itemConvertorUI.OnClickMaterialButton(_materialSlotIndex, _materialSlot);
    }
}
