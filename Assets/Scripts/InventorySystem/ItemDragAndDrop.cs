using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragAndDrop : MonoBehaviour
{
    [SerializeField] private ItemSlot _dragAndDropItem = new ItemSlot();
    [SerializeField] private RectTransform _dragAndDropIconTransform;
    [SerializeField] private Image _dragAndDropIconImage;

    private Dictionary<int, string> _equipmentPartMap;

    private void Start()
    {
        _equipmentPartMap = new Dictionary<int, string>()
        {
            { 0, "Head" },
            { 1, "Torso" },
            { 2, "Legs" },
            { 3, "Backpack" }
        };
    }

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
                SwapItem(itemSlot);
            }
        }
        UpdateIcon();
    }

    private void SwapItem(ItemSlot itemSlot)
    {
        SO_Item item = itemSlot.item;
        int quantity = itemSlot.quantity;

        itemSlot.Copy(_dragAndDropItem);
        _dragAndDropItem.Set(item, quantity);
    }

    public void OnClickOnEquipmentPanel(ItemSlot itemSlot, int id)
    {
        if (_dragAndDropItem.item == null)
        {
            _dragAndDropItem.Copy(itemSlot);
            itemSlot.Clear();
        }
        else
        {
            if (itemSlot.item.itemType != ItemTypes.Armor) return;
            if (!_equipmentPartMap.TryGetValue(id, out string expectedPart)) return;
            if (_dragAndDropItem.item.armorData.partName != expectedPart) return;

            SwapItem(itemSlot);
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
