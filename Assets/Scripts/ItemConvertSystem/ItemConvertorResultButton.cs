using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemConvertorResultButton : MonoBehaviour
{
    [SerializeField] private ItemConvertorUI _itemConvertorUI;
    [SerializeField] private Image _resultIcon;
    [SerializeField] private TMP_Text _quantity;
    [SerializeField] private ItemDragAndDrop _dragAndDrop;
    [SerializeField] private Button _resultButton;

    private ItemSlot _materialSlot = new ItemSlot();

    private void Start()
    {
        _resultButton.onClick.AddListener(() => OnClick(_materialSlot));
    }

    public void Show(ItemSlot slot)
    {
        _materialSlot.Copy(slot);
        _resultIcon.gameObject.SetActive(true);
        _resultIcon.sprite = _materialSlot.item.itemIcon;
        _quantity.gameObject.SetActive(true);
        _quantity.text = _materialSlot.quantity.ToString();
    }

    public void Clear()
    {
        _resultIcon.sprite = null;
        _resultIcon.gameObject.SetActive(false);
        _quantity.text = "";
        _quantity.gameObject.SetActive(false);
    }

    public void OnClick(ItemSlot itemSlot)
    {
        _dragAndDrop.OnClick(itemSlot);
    }
}
