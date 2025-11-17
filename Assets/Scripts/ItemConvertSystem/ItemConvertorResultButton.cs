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

    [SerializeField] private ItemSlot _resultSlot = new ItemSlot();

    private void Start()
    {
        _resultButton.onClick.AddListener(OnClick);
    }

    public void Show(ItemSlot slot)
    {
        _resultSlot.Copy(slot);
        _resultIcon.gameObject.SetActive(true);
        _resultIcon.sprite = _resultSlot.item.itemIcon;
        _quantity.gameObject.SetActive(true);
        _quantity.text = _resultSlot.quantity.ToString();
    }

    public void Clear()
    {
        _resultIcon.sprite = null;
        _resultIcon.gameObject.SetActive(false);
        _quantity.text = "";
        _quantity.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        _dragAndDrop.OnClick(_resultSlot);
        _itemConvertorUI.OnClickResultButton(_resultSlot);
    }
}
