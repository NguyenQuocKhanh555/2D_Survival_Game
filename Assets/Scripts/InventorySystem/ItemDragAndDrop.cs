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

    private Dictionary<int, PlayerBodyPartType> _equipmentPartMap;

    private void Start()
    {
        _equipmentPartMap = new Dictionary<int, PlayerBodyPartType>()
        {
            { 0, PlayerBodyPartType.Head },
            { 1, PlayerBodyPartType.Torso },
            { 2, PlayerBodyPartType.Legs },
            { 3, PlayerBodyPartType.Backpack },
            { 4, PlayerBodyPartType.Ring },
            { 5, PlayerBodyPartType.Ring }
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
            if (_dragAndDropItem.item == itemSlot.item && itemSlot.item.isStackable) 
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
            if (_dragAndDropItem.item.itemType != ItemTypes.Armor) return;
            if (!_equipmentPartMap.TryGetValue(id, out PlayerBodyPartType expectedPart)) return;
            if (_dragAndDropItem.item.armorData.partName != expectedPart) return;

            SwapItem(itemSlot);
        }
        UpdateIcon();
    }

    public void OnClickOnResearchTablePanel(ItemSlot itemSlot)
    {
        SO_Item dragItem = _dragAndDropItem.item;
        if (dragItem == null)
        {
            _dragAndDropItem.Copy(itemSlot);
            itemSlot.Clear();
            UpdateIcon();
            return;
        }

        if (dragItem.itemType != ItemTypes.Material) return;

        SO_Item slotItem = itemSlot.item;
        bool isDragItemStackable = dragItem.isStackable;

        if (slotItem == null)
        {
            if (isDragItemStackable)
            {
                itemSlot.TakeOne(_dragAndDropItem);
            }      
            else
            {
                itemSlot.Copy(_dragAndDropItem);
                _dragAndDropItem.Clear();
                UpdateIcon();
            }
            UpdateIcon();
            return;
        }

        if (slotItem == dragItem)
        {
            if (isDragItemStackable)
            {
                _dragAndDropItem.quantity += 1;
                itemSlot.Clear();
            }
            return;
        }

        if (!isDragItemStackable)
        {
            SwapItem(itemSlot);
            UpdateIcon();
        }      
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
