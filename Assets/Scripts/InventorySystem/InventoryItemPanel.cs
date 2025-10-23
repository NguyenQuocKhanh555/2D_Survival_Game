using System.Collections.Generic;
using UnityEngine;

public class InventoryItemPanel : MonoBehaviour, IItemPanel
{
    [SerializeField] private SO_ItemContainer _inventoryContainer;
    [SerializeField] private List<InventoryItemButton> _buttons;
    [SerializeField] private ItemDragAndDrop _itemDragAndDrop;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetSourcePanel();
        SetIndex();
        Show();
    }

    private void OnEnable()
    {
        Clear();
        Show();
    }

    private void LateUpdate()
    {
        if (_inventoryContainer == null) return;

        if (_inventoryContainer.isChange)
        {
            Show();
            _inventoryContainer.isChange = false;
        }
    }

    public void SetSourcePanel()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].SetItemPanel(this);
        }
    }

    public void SetIndex()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].SetInventoryIndex(i);
        }
    }

    public void OnClick(int id)
    {
        _itemDragAndDrop.OnClick(_inventoryContainer.slots[id]);
        Show();
    }

    public void Show()
    {
        if (_inventoryContainer == null) return;

        for (int i = 0; i < _inventoryContainer.slots.Count; i++)
        {
            if (_inventoryContainer.slots[i].item == null)
            {
                _buttons[i].Clear();
            }
            else
            {
                _buttons[i].Set(_inventoryContainer.slots[i]);
            }
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            _buttons[i].Clear();
        }
    }
}
