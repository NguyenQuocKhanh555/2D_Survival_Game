using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDrop : MonoBehaviour
{
    [SerializeField] private ItemSlot _dragAndDropItem = new ItemSlot();
    [SerializeField] private RectTransform _dragAndDropIconTransform;
    [SerializeField] private Image _dragAndDropIconImage;

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            _dragAndDropIconTransform.position = Input.mousePosition;

            if (Input.GetMouseButton(0))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPosition.z = 0;

                    _dragAndDropItem.Clear();
                    this.gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnClick(ItemSlot itemSlot)
    {
        if (_dragAndDropItem.item == null)
        {
            _dragAndDropItem.Copy(itemSlot);
            itemSlot.Clear();
        }
        else
        {
            if (_dragAndDropItem.item == itemSlot.item) 
            {
                itemSlot.quantity += _dragAndDropItem.quantity;
                _dragAndDropItem.Clear();
            }
            else
            {
                SO_Item item = itemSlot.item;
                int quantity = itemSlot.quantity;

                itemSlot.Copy(_dragAndDropItem);
                _dragAndDropItem.Set(item, quantity);
            }
        }
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (_dragAndDropItem.item == null) 
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
            _dragAndDropIconImage.sprite = _dragAndDropItem.item.itemIcon;
        }
    }
}
